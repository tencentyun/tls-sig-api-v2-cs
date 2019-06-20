## 说明
此项目为 tls-sig-api-v2 版本的 c# 实现。之前的非对称密钥无法使用此版本的 api，如需使用请查看[这里](https://github.com/tencentyun/tls-sig-api-cs)。

## 集成
使用 Visual Studio 工具下的程序包管理器和 NuGet 命令行均可以集成。另外也可以下载源码直接集成。

### VS 集成
在 Visual Studio 中按照 `工具`->`NuGet 包管理器`->`管理解决方案的 NuGet 程序包`，然后搜索 `tls-sig-api-v2` 进行安装。

### NuGet 命令行集成

项目已经打包上传至 nuget.org 包管理仓库，可以使用 nuget 直接安装。
```
PM> Install-Package tls-sig-api-v2
```
多种命令行安装方式[这里](https://www.nuget.org/packages/tls-sig-api-v2)可以查看。

### 源码集成
将 `tls-sig-api-v2-cs/TLSSigAPIv2.cs` 下载放置到开发者项目目录下，按照下述示例代码调用即可。

## 使用
``` c#
using tencentyun;

TLSSigAPIv2 api = new TLSSigAPIv2(1400000000, "5bd2850fff3ecb11d7c805251c51ee463a25727bddc2385f3fa8bfee1bb93b5e");
string sig = api.GenSig("xiaojun");
System.Console.WriteLine(sig);
```
