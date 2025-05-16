#!/bin/bash

# 检查参数
if [ "$#" -lt 2 ]; then
  echo "❌ 用法: ./upload-bug-snapshot.sh <bug-tag> <file1> <file2> ..."
  exit 1
fi

BUG_TAG=$1
shift

# 设置本地仓库路径
REPO_DIR="/Users/yaoruibo/Desktop/Blog/Github-bug-snapshots"
REPO_URL="git@github.com:yaoruibo/github-bug-snapshots.git"

# 创建临时分支文件夹
TMP_DIR="$REPO_DIR/tmp_$BUG_TAG"
mkdir -p "$TMP_DIR"

# 拷贝文件进去
for FILE in "$@"; do
  if [ ! -f "$FILE" ]; then
    echo "⚠️ 文件 $FILE 不存在，跳过。"
    continue
  fi
  cp "$FILE" "$TMP_DIR/"
done

cd "$REPO_DIR" || exit 1
git checkout --orphan "$BUG_TAG"
rm -rf *
cp -r "$TMP_DIR"/* .
rm -rf "$TMP_DIR"

git add .
git commit -m "🐞 Snapshot for $BUG_TAG"
git push origin "$BUG_TAG"

echo ""
echo "✅ 上传成功！访问链接："
echo "👉 https://github.com/yaoruibo/github-bug-snapshots/tree/$BUG_TAG
