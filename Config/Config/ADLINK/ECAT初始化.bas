
'*******************************************************
'ECAT总线初始化变量定义

global CONST BUS_TYPE = 0				'总线类型，用于上位机区分当前模式
global CONST BUS_SLOT = 0				'槽位号0
global CONST BUS_AXISSTART = 0			'总线轴起始轴号

global Max_AxisNum						'本张控制卡支持的最大轴数
	Max_AxisNum = SYS_ZFEATURE(1)	

global Bus_InitStatus					'总线初始化完成状态
	Bus_InitStatus = -1
	
global  Bus_TotalAxisnum				'扫描到的总轴数
	Bus_TotalAxisnum = 0
	
global  Bus_PlanAxisnum				'设备设计总轴数,由上位机赋值,用于判断节点数量是否正确
	Bus_PlanAxisnum = -1

global Bus_ScanNum						'扫描次数
	Bus_ScanNum = 0
	
global Bus_PlanScanNum					'节点数量错误时,重复扫描次数
	Bus_PlanScanNum = 3
	
?"*********VarInitFinished*********"

End

'*******************************************************
'*******************************************************



'*******************************************************
'ECAT总线初始化函数
global Sub Ecat_Init()
	
	Bus_InitStatus = -1
	Bus_TotalAxisnum = 0
	Bus_ScanNum = 0
	
	RAPIDSTOP(2)
	For i=0 To MAX_AXISNUM - 1		'初始化还原轴类型					
		AXIS_ENABLE(i) = 0
		ATYPE(i)=0	
	Next
	
	'节点数量不对时,循环扫描	
	Repeat
		
		SLOT_STOP(BUS_SLOT)
		DELAY(1000)
		
		SLOT_SCAN(BUS_SLOT)											'开始扫描
		If Return Then 
			
			node_io(0,13) = 16
			
			?"总线扫描成功","连接设备数：",NODE_COUNT(BUS_SLOT)
			?
			?"开始映射轴号"
			For i=0 To NODE_COUNT(BUS_SLOT)-1						'遍历总线下所有从站节点

				If NODE_AXIS_COUNT(BUS_SLOT,i) <> 0 Then			'判断当前节点是否有电机
					
					For j=0 To NODE_AXIS_COUNT(BUS_SLOT,i)-1
						
						AXIS_ADDRESS(BUS_AXISSTART+i) = Bus_TotalAxisnum+1			'映射轴号
						ATYPE(BUS_AXISSTART+i) = 65									'设置控制模式 65-位置 66-速度 67-转矩 
						DRIVE_PROFILE(BUS_AXISSTART+i) = 4							'设置PROFILE功能
						
						DISABLE_GROUP(BUS_AXISSTART+i)								'每轴单独分组
						
						DRIVE_IO(BUS_AXISSTART+i) = 128 + (BUS_AXISSTART+i)*16		'映射驱动器上的IO状态
						REV_IN(BUS_AXISSTART+i) = 128 + (BUS_AXISSTART+i)*16 
						FWD_IN(BUS_AXISSTART+i) = 129 + (BUS_AXISSTART+i)*16
						DATUM_IN(BUS_AXISSTART+i) = 130 + (BUS_AXISSTART+i)*16
						INVERT_IN(128 + (BUS_AXISSTART+i)*16,ON)
						INVERT_IN(129 + (BUS_AXISSTART+i)*16,ON)
						INVERT_IN(130 + (BUS_AXISSTART+i)*16,ON)
						
						Bus_TotalAxisnum=Bus_TotalAxisnum+1							'总轴数+1
						
					Next
					
				EndIf
				
			Next
			
			?"轴号映射完成","连接总轴数：",Bus_TotalAxisnum
			
			
			'判断节点数量
			If Bus_PlanAxisnum <> Bus_TotalAxisnum Then 
				
				Bus_ScanNum = Bus_ScanNum + 1
				?"扫描到的节点数量错误,次数",Bus_ScanNum
				
				If Bus_ScanNum >= Bus_PlanScanNum Then 
					?"总线节点数量错误"
					Bus_InitStatus = 2
				EndIf
				
			Else
				
				'SDO可以修改电机齿轮比、电机方向
				'SDO_WRITE(0,0,$6091,01,7,8388608)	'齿轮比分子 编码器分辨率
				'SDO_WRITE(0,0,$6091,02,7,10000)	'齿轮比分母 一圈指令脉冲数
				'?"驱动器参数修改完成"
				
				DELAY(100)
				
				SLOT_START(BUS_SLOT)						'启动总线
				If Return Then 
					
					?"总线开启成功"
					
					?"开始清除驱动器错误(根据驱动器数据字典设置)"
					For i= BUS_AXISSTART To BUS_AXISSTART + Bus_TotalAxisnum - 1 
						
						DRIVE_CONTROLWORD(i)=128			'根据驱动器数据字典
						DELAY(100)
						DRIVE_CONTROLWORD(i)=6
						DELAY(100)
						DRIVE_CONTROLWORD(i)=15
						DELAY(100)
					Next
					?"驱动器错误清除完成"
					DELAY(100)

					?"清除控制器错误"
					DATUM(0)
					
					?"控制器错误清除完成"
					DELAY(100)
					
					?"轴使能准备"
					For i= BUS_AXISSTART To BUS_AXISSTART + Bus_TotalAxisnum - 1
						BASE(i)
						AXIS_ENABLE=0						'单轴使能开关
					Next
					
					WDOG=1									'总使能开关
					
					?"轴使能完成"
					Bus_InitStatus  = 1
					
					HeartCheck								'心跳检测功能
					
				Else
					
					?"总线开启失败"
					Bus_InitStatus = 0
					
				EndIf
				
			EndIf
			
		Else
		
			?"总线扫描失败"
			Bus_InitStatus = 0
			
		Endif
		
	Until(Bus_InitStatus <> -1)

end Sub



'*******************************************************
'心跳检测
global Sub HeartCheck() 
	global heartSwitch		'心跳检测开关
		heartSwitch = 0
	global scan_Value		'上位机每1s内 赋值该变量非0值
		scan_Value = 0
	global temp_Value		'scan_Value初值
		temp_Value = scan_Value
		
	INT_ENABLE=1			'初始化完成后开启中断
	TIMER_STOP(0)			'定时器0关闭
	TIMER_START(0,1000)	'定时器0开启，1000ms后执行一次 定时器中断函数
	
end Sub

'心跳检测 定时器中断函数
global Sub ONTIMER0()  
	'心跳检测 掉线停机开关
	If heartSwitch = 1 Then 
		If scan_Value = temp_Value Then		'检测和上一秒的值是否相等,若掉线则停止所有轴
			RAPIDSTOP(2)						
			?"控制器掉线！！！"
		EndIf
	EndIf
	
	scan_Value = temp_Value						'复位变量值
	
	TIMER_START(0,1000)						'再次开启定时器,自循环
	
end Sub
