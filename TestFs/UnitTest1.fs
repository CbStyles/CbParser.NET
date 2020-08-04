module TestFs

open NUnit.Framework

open CbStyle.Parser

[<SetUp>]
let Setup () =
    ()

[<Test>]
let Test1 () =
    let a = [|1; 2; 3|]
    let s = Span a
    let a = s.[..1]
    printf "%s" (a.ToString())
    Assert.Pass()
