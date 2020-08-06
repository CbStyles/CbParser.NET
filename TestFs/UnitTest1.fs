module TestFs

open NUnit.Framework

open CbStyles.Parser

[<SetUp>]
let Setup () =
    ()

[<Test>]
let TestSpan1 () =
    let a = [| 1; 2; 3; 4; 5|]
    let s = Span a
    let a = s.[1..3]
    printf "%s" (a.ToString())
    Assert.AreEqual(a, [| 2; 3 |])

[<Test>]
let TestSpan2 () =
    let a = [| 1; 2; 3; 4; 5 |]
    let s = Span a
    let a = s.[..3]
    printf "%s" (a.ToString())
    Assert.AreEqual(a, [| 1; 2; 3 |])

[<Test>]
let TestSpan3 () =
    let a = [| 1; 2; 3; 4; 5 |]
    let s = Span a
    let a = s.[2..]
    printf "%s" (a.ToString())
    Assert.AreEqual(a, [| 3; 4; 5 |])

[<Test>]
let TestSpan4 () =
    let a = [| 1; 2; 3 |]
    let s = Span a
    let a = s.TryGet(3);
    printf "%s" (a.ToString())
    Assert.AreEqual(a, ValueOption<int32>.ValueNone)

[<Test>]
let TestSpan5 () =
    let a = [| 1; 2; 3 |]
    let s = Span a
    let a = s.TryGet(2);
    printf "%s" (a.ToString())
    Assert.AreEqual(a, ValueSome(3))