open Loader

[<EntryPoint>]
let main args = 
    // 开始
    printfn "Now compiling %s..." args.[0]
    
    // 加载
    Loader.load args.[0]
    // 语法分析
    |> Parser.parse 

    // return
    0 