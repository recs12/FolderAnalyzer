#r "nuget: Deedle, 2.5.0"
open Deedle
open System
open System.IO
open FSharp.Collections
open FSharp.Core

// argu library.
// generate report xlsx file.
// use deedle
// read xml
// read json

type Row = { Folder: string; Request: string; Date: DateTime; IsDocuments:List<int>; Uii:string}
// type Row = { Folder: string; Request: string; Date: DateTime; IsXml:int; IsJson:int; IsPDF:int; Uii:string}

let glob folder  = (folder, "*", SearchOption.TopDirectoryOnly) |> Directory.GetFileSystemEntries
let globfile folder extension = (folder, extension, SearchOption.AllDirectories) |> Directory.GetFileSystemEntries
let getName (pathtoFile:string) =  pathtoFile |> IO.Path.GetFileName
let getDate (pathtoFile:string) =  pathtoFile |> Directory.GetCreationTime
let getLength path extension  = (globfile path extension) |> Array.length
let getUii (pathtoFile:string) =  pathtoFile |> IO.Path.GetFileNameWithoutExtension


let getUiiNumber path = 
    match (globfile path "*.pdf").Length with
    | 0 -> "-"
    | 1 -> (globfile path "*.pdf")[0] |> getUii
    | _ -> "?"


glob @"C:\Users\recs\OneDrive - Premier Tech\Bureau\Dispatcher\# 5-12-2022"
|> Array.Parallel.map (fun path -> 
    {
        Date = getDate path
        Folder = path
        Request = getUii path
        IsDocuments = ["*.xml";"*.json";"*.pdf";"*.png"] |> List.map (fun x -> getLength path x) 
        Uii = getUiiNumber path
    })
|> Array.sortBy (fun x -> x.Date)  
|> Array.iter (fun x -> printfn $"{x.Date}\t{x.Request}\t{x.IsDocuments[0]}{x.IsDocuments[1]}{x.IsDocuments[2]}{x.IsDocuments[3]}\t{x.Uii}")





