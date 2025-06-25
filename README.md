# Jh.Admin.NETCore

**Jh.Admin.NETCore** 是一个基于 `MySQL` + `.NET 7` + `Entity Framework Core (EF Core)` + `Code First` + `Fluent API` 构建的后台管理系统后端模板。

<br>
<br>


## ✨ 技术选型

- **ASP.NET Core Web API**：跨平台的高性能 Web 服务框架  
- **Entity Framework Core (EF Core)**：使用 Code First 模式进行数据库建模，结合 Fluent API 实现灵活的表结构配置  
- **MySQL**：开源高性能关系型数据库  
- **AutoMapper**：实体与 DTO 自动映射  
- **FluentValidation**：模型验证框架  
- **JWT**：基于令牌的身份验证机制  
- **Swagger**：API 文档自动生成与调试工具  

<br>

## 🧩 核心特性

- 🔧 **Code First + Fluent API**：通过代码定义实体结构与关系，使用 Fluent API 灵活配置主键、索引、多对多关系等数据库行为  
- 🔐 **权限管理**：用户 ⇄ 角色 ⇄ 菜单 ⇄ 按钮 四级权限控制，支持按钮级别的接口权限判断  
- 🔄 **接口标准化**：统一的 API 响应格式，支持动态菜单、权限控制等  
- 📦 **模块化架构设计**：职责清晰，易于开发维护和功能扩展  

<br>

## 📁 项目结构

```text

Jh.Admin.NETCore
├── Admin.NETCore.API // 接口入口层（API 层），Web API 入口
├── Admin.NETCore.Core // 核心业务层，Interface、Service、ViewModel、DTO等
├── Admin.NETCore.Infrastructure // 基础设施层，数据库、Fluent API配置、Entity、Migrations等
└── Admin.NETCore.Common // 通用功能层，工具类、常量、扩展方法、枚举等
```

<br>

## ⚙️ 接口功能模块

- ✅ **用户管理接口**：用户增删改查、角色授权
- ✅ **角色管理接口**：角色增删改查 


<br>

## 📎 接口文档地址
📄 [点击查看接口文档](https://www.showdoc.com.cn/2598841408187033/11558780359586032)

<br>
