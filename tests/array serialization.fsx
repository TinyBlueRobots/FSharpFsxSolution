#load "References.fsx"

[<SetUp>]
let setUp() = printfn "I'm going to serialize some fruit!"

[<TearDown>]
let tearDown() = printfn "Complete"

[<Test>]
let ``serializes array``() =
  [| "apple" ; "banana" |] |> ArraySerializer.serialize == """["apple","banana"]"""

[<TestCase [| "apple" ; "banana" |]>]
let ``serializes some strings`` strings =
  ArraySerializer.serialize strings == """["apple","banana"]"""