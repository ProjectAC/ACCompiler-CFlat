module ParserCombinator

open System

// ------------------------------------------------------------- //
// 状态机

type Position = {
    row:        int
    column:     int
}

type State = {
    text:       string
    pointer:    int
    position:   Position
}

let initialState (s: string) = {
        text        = s
        pointer     = 0
        position    = {
            row         = 1
            column      = 1
        }
    }

/// 读取当前状态对应的字符
let getChar state = 
    state.text.Chars state.pointer

type OutOfRange (p: Position) =
    inherit ApplicationException()

/// 进行一步状态转换
/// 具体而言就是获取一个字符，并且把指针后移
let step state =
    match state with
    | state when state.pointer = state.text.Length ->
        raise (OutOfRange (state.position))
    | state ->
        (
            getChar(state),
            match getChar(state) with
            // 换行
            | (c) when c = '\n' ->
                {
                    text        = state.text
                    pointer     = state.pointer + 1
                    position    = {
                        row         = state.position.row + 1
                        column      = 1
                    }
                }
            // 不换
            | _ ->
                {
                    text        = state.text
                    pointer     = state.pointer + 1
                    position    = {
                        row         = state.position.row
                        column      = state.position.column + 1
                    }
                }
        )

let rec fetch state ptr =
    match state with
    | state when state.pointer = ptr ->
        ""
    | _ ->
        let (c, next) = step state
        string(c) + fetch next ptr

// ------------------------------------------------------------- //
// 函数调用

type ParserC = State option -> State option

let call f arg =
    match arg with
    | Some s        -> f s
    | None          -> None

let (<+>) f1 f2 = 
    f1 >> f2

let (<|>) f1 f2 = function
    | Some s ->
        match f1 (Some s) with
        | Some t    -> Some t
        | None      -> f2(Some s)
    | None   -> None

let recurse f = 
    let rec rf (s: State option) = 
        try
            match f s with
            | Some s    -> rf (Some s)
            | None      -> s
        with
            | :? OutOfRange -> s
    rf

let recurse2 f1 f2 = 
    let rec rf (s: State option) = 
        try
            match f1 s with
            | Some _    -> s
            | _         -> 
                match f2 s with
                | Some s    -> rf (Some s)
                | _ -> None
        with
            | :? OutOfRange -> None
    rf

let next = function
    | Some s ->
        let (_, t) = step(s)
        Some t
    | None -> None

// ------------------------------------------------------------- //
// 匹配过程

let charEQ c = 
    call (
        fun s ->
            let (cur, t) = step(s)
            if cur = c then Some t else None
    )
let charNE c = 
    call (
        fun s ->
            let (cur, t) = step(s)
            if cur <> c then Some t else None
    )

let rec stringRec s1 s2 = 
    match s1 with
    | s1 when s1.pointer = s1.text.Length ->
        Some s2
    | _ ->
        match (step(s1), step(s2)) with
        | ((c1, t1), (c2, t2)) when c1 = c2 ->
            stringRec t1 t2
        | _ ->
            None
let stringEQ text =
    call (
        fun s -> 
            stringRec (initialState text) s
    )
    
let blank =
    charEQ ' ' <|>
    charEQ '\n' <|>
    charEQ '\t' <|>
    charEQ '\r'
let blanks = recurse blank

let parseSegm = 
    charEQ '(' <|>
    charEQ ')' <|>
    charEQ '[' <|>
    charEQ ']' <|>
    charEQ '<' <|>
    charEQ '>' <|>
    charEQ ',' <|>
    charEQ '.' <|>
    charEQ '&' <|>
    charEQ '*' <|>
    blank

let digitRec s = 
    match step s with
    | (c, t) when '0' <= c && c <= '9' ->
        Some t
    | _ -> None
let digit = call digitRec
let digits = recurse digit

let upper s = 
    match step s with
    | (c, t) when 'A' <= c && c <= 'Z' ->
        Some t
    | _ -> None
let lower s = 
    match step s with
    | (c, t) when 'a' <= c && c <= 'z' ->
        Some t
    | _ -> None
let alpha = call upper <|> call lower
let alphas = recurse alpha

let charAny = 
    call (
        fun s ->
            let (_, t) = step s
            Some t
    )

let charCon = (charEQ '\\' <+> charAny) <|> charAny

// ------------------------------------------------------------- //
// 接口

let appendL f = function
    | Some x ->
        try
            match f (Some x) with
            | Some _ -> Some x
            | _ -> None
        with
        | :? OutOfRange -> Some x
    | _ -> None

let BooleanConstant = stringEQ "false" <|> stringEQ "true"
let UIntConstant = digit <+> digits
let IntConstant = (charEQ '+' <|> charEQ '-') <+> UIntConstant
let FloatConstant = (IntConstant <|> UIntConstant) <+> charEQ '.' <+> UIntConstant
let RationalConstant = (IntConstant <|> UIntConstant) <+> charEQ '/' <+> UIntConstant
let CharConstant = charEQ '\'' <+> charCon <+> charEQ '\''
let StringConstant = charEQ '\"' <+> (recurse2 (charEQ '\"') charCon) <+> next
let Constant = 
    BooleanConstant <|>
    UIntConstant <|>
    IntConstant <|>
    FloatConstant <|>
    RationalConstant <|>
    CharConstant <|>
    StringConstant

let Identifier = alpha <+> recurse (alpha <|> digit <|> charEQ '_')

let FunctionDefinition = 
    Identifier <+> stringEQ ":" <+> Type list

let Module = 