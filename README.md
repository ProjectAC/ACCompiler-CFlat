# ACCompilerCollection - C Flat

Another Complex Compiler Collection，又一个复杂的编译器组合。  
第一个目标语言是C♭（♭是降调的意思，念Flat，对应的♯是升调……等下C降调不是B么）。当然不知道会不会是最后一个语言。  
作为ProjectAC项目的一员，和它的名字正好相反，它将一点也不会复杂。

## 关于C Flat语言

- 整体
  - 语法类JS
  - 半函数式（JS风格）面向过程
  - 支持与C语言混编（具体方式为编译到汇编后调用C的库函数）
- 数据类型
  - 强静态类型
  - 基本数据类型：bool，int, float, char
  - 进阶数据类型：指针，函数（没错函数也视为变量）
  - 支持struct，union，enum
  - 支持struct内函数（不支持static）
  - 不支持数组、引用（统一用指针）
- 流程控制
  - if / else
  - C style for
  - while
  - do while
  
```
// 一个Sample
function int main = () => {
    int n
    // 这里调用C语言函数
    scanf("%d", &n)
    
    int res = 1
    for (int i = 1; i <= n; i = i + 1) {
        res = res * i
    }
    
    // 这里调用C语言函数
    printf("%d\n", res)
    
    return 0
}

```

对应的文法分析过程（是计算理论意义下的，不是编译器意义下的）：

```
declaration
```

```
function-declaration
```

```
type id = () => {
    statements
}
```

```
type id = () => {
    statement
    function-call
    
    declaration
    for-loop
    
    function-call
    
    return-statement
}
```

```
type id = () => {
    type id
    id(string-constant, operator id)
    
    type id = int-constant
    for (declaration; statement; statement) {
        statements
    }
    
    id(string-constant, id)
    
    return int-constant
}
```

```
type id = () => {
    type id
    id(string-constant, operator id)
    
    type id = int-constant
    for (type id = int-constant; id op id; id = id op id) {
        id = id op od
    }
    
    id(string-constant, id)
    
    return int-constant
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

当然是C。  
必要的时候会用一些奇怪的工具，比如shell脚本。可以考虑与C++混编。

## 项目架构

*待补充*  

## 备注

*待补充*

