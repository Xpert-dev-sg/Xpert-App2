**开发环境**<br/>
VS2022<br/> Net6.0<br/> WPF<br/><br/>
**DB**<br/>
SQLite<br/>
<br/>
**Dependencies**:<br/>
HandyControl<br/>
System.Data.SQLite<br/>
log4net<br/>
<br/>
**硬件接口**<br/>
<br/>
<br/>
**主菜单页面**<br/>
>轮巡展示(done)<br/>
>content list 没有权限限制，15秒（可设置）会自动回到主菜单(done)<br/>
>access 和admin 页面要输入密码方可进入(done)<br/>
>在用户login后静止60秒（可设置）会自动退出，并回到主菜单(done)<br/>

**Content list 页面**<br/>
>呈现content list(done)<br/>
>查询content list(done)<br/>

**Access 页面**<br/>
>开门前:负责普通用户的开门，并呈现该用户状态 和 该用户未还的item (done)<br/>
>关门后:会呈现本次开关门的借还item的状态，并发email<br/>

**Admin页面**<br/>
>管理用户页面 (done)增删改查 四大功能<br/>
>管理用户页面 (done)增删改查 四大功能<br/>
>log 展示页面(done)和查询<br/>

**系统服务**<br/>
>每个item有权限和部门管控，不同部门或不够权限的用户拿了会被记录然后email通知部门负责人<br/>
>每个item有return时间的限制，超时会email负责人和当事人<br/>

**截图**<br/>
>![login](https://github.com/Xpert-dev-sg/Xpert-App2/blob/main/print_screen/login.png)<br/>
>![item](https://github.com/Xpert-dev-sg/Xpert-App2/blob/main/print_screen/item.png)<br/>
>>![contentlist](https://github.com/Xpert-dev-sg/Xpert-App2/blob/main/print_screen/contentlist.png)<br/>
>![open](https://github.com/Xpert-dev-sg/Xpert-App2/blob/main/print_screen/open.png)<br/>
>>![access](https://github.com/Xpert-dev-sg/Xpert-App2/blob/main/print_screen/access.png)<br/>
>![admin](https://github.com/Xpert-dev-sg/Xpert-App2/blob/main/print_screen/admin.png)<br/>
>>![itemlist](https://github.com/Xpert-dev-sg/Xpert-App2/blob/main/print_screen/itemlist.png)<br/>
>>![userlist](https://github.com/Xpert-dev-sg/Xpert-App2/blob/main/print_screen/userlist.png)<br/>
>>![loglist](https://github.com/Xpert-dev-sg/Xpert-App2/blob/main/print_screen/loglist.png)<br/>
![aboutme](https://github.com/Xpert-dev-sg/Xpert-App2/blob/main/print_screen/aboutme.png)<br/>





**脑图**
![mindmap](https://github.com/Xpert-dev-sg/Xpert-App2/blob/main/RFID%2Bsystem.png)
