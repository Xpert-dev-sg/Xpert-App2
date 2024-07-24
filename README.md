**开发环境**：VS2022 net6.0 wpf<br/>
**DB**：sqlite

**主菜单页面**<br/>
>轮巡展示(done)<br/>
>content list 没有权限限制，15秒（可设置）会自动回到主菜单(done)<br/>
>access 和admin 页面要输入密码方可进入(done)<br/>
>在用户login后静止60秒（可设置）会自动退出，并回到主菜单<br/>

**Access 页面**<br/>
>开门前:负责普通用户的开门，并呈现该用户状态 和 该用户未还的item <br/>
>关门后:会呈现本次开关门的借还item的状态，并发email<br/>

**Admin页面**<br/>
>管理用户和item页面 增删改查 四大功能<br/>
>log 的查询<br/>

**系统服务**<br/>
>每个item有权限和部门管控，不同部门或不够权限的用户拿了会被记录然后email通知部门负责人<br/>
>每个item有return时间的限制，超时会email负责人和当事人<br/>

**脑图**
![mindmap]()
