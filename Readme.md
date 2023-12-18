#### 介绍

当前，以QFramework为主

1. YooAssetKit：对YooAsset进行支持
2. LubanKit：对Luban进行支持
3. 内置UniTask



#### 计划

- [ ] Hybrid支持
- [ ] 资源代码热更新支持

当前需要强制使用YooAsset，否则会报错，无法实现资源管理的快速插拔。根据具体的开发环境扩展。



#### LubanKit

生成代码放在LubanKit下Code文件夹下

生成数据放在Config/Json下，实际有资源系统决定，可在相关代码里调整加载过程。