# 微尘咨询平台 (Dust Info Platform)

> 一个基于 ASP.NET MVC + Entity Framework + SQL Server 的在线咨询与社区平台，支持用户注册登录、帖子发布与评论、实时聊天室、AI 助手、管理后台等功能。本项目为大二下学期软件工程课程小组作业，由四位成员分工协作完成。

---

## 项目简介

微尘咨询平台是一个集社区交流、即时通讯、智能助手于一体的 Web 应用。用户可注册账号、发布帖子、评论互动，也可进入三个固定主题的聊天室进行实时交流，或使用 AI 助手获取自动回复（基于 Ollama 本地模型）。管理员具备封禁用户、管理帖子与反馈等后台功能，并可利用 AI 整理邮件。系统部署时通过 ngrok 内网穿透实现公网访问。

**项目背景**：大二下学期软件工程课程设计，小组名“摆烂”，成员包括胡孜恒（组长）、高展延、艾子随、苏永浩。项目覆盖了需求分析、系统设计、编码实现、测试和答辩的完整流程。

---

## 功能特性

- 用户注册与登录：采用盐加哈希加密存储密码，防止彩虹表攻击
- 个人中心：修改密码、修改个人信息、管理自己的帖子
- 帖子管理：发布帖子、查看帖子详情、发表评论、删除帖子、模糊查询
- 聊天室：三个固定聊天室（基于 JavaScript + 后端 WebSocket 实现实时通信）
- AI 助手：
  - 用户端：集成 Ollama 本地大模型，通过 JSON 格式请求/响应实现智能对话
  - 管理端：利用 AI 整理邮件，提升后台工作效率
- 消息模块：站内消息通知
- 用户反馈：用户可提交反馈信息
- 管理后台（预设管理员账号）：
  - 用户管理（封禁/解封）
  - 帖子管理
  - 反馈管理
- 部署方式：本地 IIS 服务器 + ngrok 内网穿透实现公网访问

---

## 技术栈

| 技术 | 说明 |
|------|------|
| 编程语言 | C# |
| 框架 | ASP.NET MVC (.NET Framework) |
| ORM | Entity Framework（默认版本） |
| 数据库 | SQL Server（数据库名：YouXi） |
| 实时通信 | WebSocket（JavaScript + 后端） |
| AI 集成 | Ollama 本地模型，JSON 数据交互 |
| 前端 | jQuery、Bootstrap、自定义 CSS |
| 测试工具 | Python 脚本（自动化接口测试） |
| 部署 | 本地 IIS + ngrok 内网穿透 |
| 安全 | 密码盐加哈希存储 |

---

## 项目目录结构

```
Dust-Info-Platform/
├── 1 项目规格/                # 项目规格说明文档
├── 2 项目立项/                # 项目立项文档（含小组分工表）
├── 3 项目计划/                # 开发计划
├── 4 项目需求/                # 需求规格说明书
├── 5 项目设计/                # 系统设计文档、数据库设计（含数据字典）
├── 6 项目源码/                # 源代码
│   ├── WebApplication1/       # 主项目
│   │   ├── Controllers/       # 控制器（如 HomeController、LiaoTianController、GuanLiController 等）
│   │   ├── Models/            # 模型（包含 EF 生成的实体类）
│   │   ├── Views/             # 视图页面（.cshtml）
│   │   ├── Services/          # 服务层（如 LiaoTianService 等，含 AI 调用逻辑）
│   │   ├── Content/           # 静态资源（CSS、JS、图片等）
│   │   ├── Web.config         # 配置文件（含数据库连接字符串）
│   │   └── ...
│   └── 测试代码/              # Python 测试脚本（高展延编写）
├── 8 项目答辩/                # 答辩 PPT
└── 9 项目总结/                # 项目总结报告
```

注：数据库文件未上传至仓库，但完整设计见 `5 项目设计` 文件夹。

---

## 小组分工（组名：摆烂）

| 成员 | 角色 | 负责模块 |
|------|------|----------|
| 胡孜恒 | 项目组长 | 所有项目安全性检查、登录模块、个人中心（修改密码/信息）、聊天室模块（WebSocket）、AI助手模块（Ollama 集成）、用户反馈模块、管理端全部功能、项目部署 |
| 高展延 | 组员 | 全部代码的测试（编写 Python 自动化测试脚本） |
| 艾子随 | 组员 | 帖子管理模块下的查询功能、个人中心模块的帖子修改和删除 |
| 苏永浩 | 组员 | 用户模块、消息模块、帖子管理模块的帖子详情和评论、帖子添加功能 |

---

## 快速开始

### 环境要求

- Windows 操作系统
- Visual Studio 2019 或更高版本（需安装 ASP.NET 和 Web 开发组件）
- SQL Server（本地或远程实例，数据库名为 YouXi）
- Ollama（需本地安装并拉取对话模型，用于 AI 助手）
- IIS（用于本地部署，可选）
- ngrok（用于内网穿透，可选）

### 本地运行步骤

1. 克隆仓库
   ```bash
   git clone https://github.com/pmxw2006/Dust-Info-Platform.git
   ```

2. 在 SQL Server 中新建名为 `YouXi` 的数据库，根据 `5 项目设计` 文件夹下的数据库设计文档创建表结构。

3. 修改 `Web.config` 中的连接字符串，指向你的数据库实例：
   ```xml
   <connectionStrings>
     <add name="YouXiEntities" connectionString="metadata=...;provider=System.Data.SqlClient;provider connection string=&quot;data source=.;initial catalog=YouXi;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />
   </connectionStrings>
   ```
   根据你的环境调整 `data source` 和认证方式（示例使用 Windows 集成验证）。

4. 确保本地已安装 Ollama 并运行所需模型（如 llama2 或项目配置的模型）。AI 模块通过 JSON 请求本地 Ollama API 地址（通常为 `http://localhost:11434`），可在代码中检查配置是否正确。

5. 使用 Visual Studio 打开 `6 项目源码/WebApplication1/WebApplication1.sln`。

6. 按 F5 运行项目，系统将自动编译并在浏览器中打开主页。

### 部署到 IIS 并实现公网访问

1. 在 Visual Studio 中发布项目到本地文件夹。
2. 在 IIS 中创建网站，指向发布文件夹。
3. 启动 ngrok 进行内网穿透：
   ```bash
   ngrok http 你的IIS端口号
   ```
4. 将 ngrok 生成的公网地址分享给组员或老师即可访问。

---

## 安全性说明

- 密码存储：使用盐加哈希方式加密，防止彩虹表攻击。
- 数据库访问：使用 Entity Framework，默认生成参数化查询，可防止 SQL 注入。
- 管理员账号：系统预设，不开放注册，避免权限提升风险。
- 连接字符串：存储在 `Web.config` 中，部署时注意环境隔离，避免泄露。
- 输入验证：前端和后端均进行了一定程度的校验，但仍需加强非法字符过滤和请求频率限制。
- AI 调用安全：AI 模块通过服务端调用本地 Ollama 实例，数据不经过第三方平台，用户隐私有保障。

---

## 模型与设计反思

- 架构模式：项目遵循 ASP.NET MVC 约定，配合 Services 处理业务逻辑，结构清晰。
- 实时聊天：通过 WebSocket 实现三个固定聊天室的实时通信，提升了用户体验。
- AI 集成：主动引入 Ollama 本地的deepssk-R1：8B模型，实现了用户端智能对话和管理端邮件整理，为大二项目中的亮点技术探索。
- 测试体系：由高展延编写的 Python 测试脚本实现了接口自动化测试，保障主要功能稳定性。
- 数据库设计：详见 `5 项目设计` 文件夹，包含 ER 图和数据字典，设计合理。
- 可提升点：部分控制器代码可进一步瘦身；前端页面响应式适配可优化；可为 API 增加速率限制防止滥用。

---

## 作者与贡献

- **pmxw2006（缥缈）** - 项目主要开发者（胡孜恒）
- 小组成员：高展延、艾子随、苏永浩
- 详细分工见上方表格

---

## 致谢

感谢课程指导老师在项目开发过程中给予的指导和帮助，感谢小组全体成员的协作与付出。

---

## 许可证

本项目仅用于学习交流，未经许可不得用于商业用途。

---

*最后更新：2026年5月*
