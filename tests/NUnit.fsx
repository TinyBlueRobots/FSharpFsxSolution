[<AutoOpen>]
module NUnit

#load "References.fsx"
open NUnit.Framework

let (==) actual expected = Assert.AreEqual(box expected, box actual)
let (!=) actual expected = Assert.AreNotEqual(box expected, box actual)

type Test = TestAttribute

type SetUp = SetUpAttribute

type TearDown = TearDownAttribute

type TestCase = TestCaseAttribute