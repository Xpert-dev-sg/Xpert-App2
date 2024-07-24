开发环境：VS2022 net6.0 wpf
db：sqlite

主菜单页面
轮训展示
content list 没有权限限制，15秒（可设置）会自动回到主菜单
access 和admin 页面要输入密码方可进入
在用户login后静止60秒（可设置）会自动退出，并回到主菜单

access 页面
开门前
负责普通用户的开门，并呈现该用户状态
该用户未还的item 
关门后 
会呈现本次开关门的借还item的状态，并发email

admin页面
管理用户和item页面 增删改查 四大功能
log

系统服务
每个item有权限和部门管控，不同部门或不够权限的用户拿了会被记录然后email通知部门负责人
每个item有return时间的限制，超时会email负责人和当事人
