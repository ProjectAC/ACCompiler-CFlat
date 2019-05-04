# ACCompilerCollection - C Flat

Another Complex Compiler Collection，又一个复杂的编译器组合。  
第一个目标语言是C♭（♭是降调的意思，念Flat，对应的♯是升调……等下C降调不是B么）。当然不知道会不会是最后一个语言。  
作为ProjectAC项目的一员，和它的名字正好相反，它将一点也不会复杂。

## 关于C Flat语言

**注意，本节内容已经经过大幅度修改！**

- 整体
  - 语法独特
  - 纯函数式
  - 支持与C语言混编（具体方式为编译到汇编后调用C的库函数）
  - 完全依赖C语言编译器
- 数据类型
  - 强静态类型
  - 基本数据类型：bool，int, float, char
  - 进阶数据类型：list，tuple，struct（预定义tuple），union，function
- 流程控制
  - match
  
```
// 一个Sample：求n!
import io

/* 定义函数时：
 * f: a -> b = ... ：f是参数为a，返回类型为b的函数
 * f: b = ...      ：f是没有参数，返回类型为b的函数
 * f = ...          : f是没有参数，返回类型自动推导的函数
 */

// 求阶乘
let fact: (n: int) -> int = {
  match n with
  | m when m < 0 ->
    nil
  | a when a <= 1 ->
    1
  | others -> 
    n * fact(n-1)
}

// 主函数
let main: int = {
  read('%d')
  |> fact
  |> write('%d')

  0
}

```

## 进度

完成请加粗。  
想添加子项请随意。  

- 语法设计完善
- 文法分析器
- 语义分析器
- 代码生成器（目标x86汇编）
- 汇编器（初步计划用系统自带汇编器完成最后一步）

## 开发语言

主要为F#。

## 项目架构

*待补充*  

## 备注

*待补充*

