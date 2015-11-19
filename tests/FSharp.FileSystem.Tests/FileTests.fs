module FSharp.FileSystem.Tests.FileTests

open FSharp.FileSystem
open FSharp.FileSystem.Path
open NUnit.Framework

let create() =
    let p = Path.tempFile()
    File.writeAllText (p) "placeholder"
    p

let check path =
    let contents = File.readAllText path
    if contents <> "placeholder" then
        failwith "contents were not \"placeholder\""

let checkNot path =
    let contents = File.readAllText path
    if contents = "placeholder" then
        failwith "contents were \"placeholder\""

let createEmpty() =
    Path.tempFile()

let tempFile() =
    Path.tempPath() @@ System.Guid.NewGuid().ToString()

[<Test>]
let ``copy skip - no exist`` () =
    let source = create()
    let dest = tempFile()
    File.copyTo source dest File.Skip
    check dest

[<Test>]
let ``copy skip - exist`` () =
    let source = create()
    let dest = createEmpty()
    File.copyTo source dest File.Skip
    checkNot dest

[<Test>]
let ``copy overwrite - no exist`` () =
    let source = create()
    let dest = tempFile()
    File.copyTo source dest File.Skip
    check dest

[<Test>]
let ``copy overwrite - exist`` () =
    let source = create()
    let dest = createEmpty()
    File.copyTo source dest File.Overwrite
    check dest


[<Test>]
let ``copy fail - no exist`` () =
    let source = create()
    let dest = tempFile()
    File.copyTo source dest File.Skip
    check dest

[<Test>]
let ``copy fail - exist`` () =
    let source = create()
    let dest = createEmpty()
    Assert.Throws(typedefof<System.IO.IOException>, fun () -> File.copyTo source dest File.Fail)
    |> ignore