(* Copyright (c) 2013 MongoDB, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *)

namespace FSharp.MongoDB.Bson.Serialization.Serializers

open System.Collections.Generic

open MongoDB.Bson.Serialization
open MongoDB.Bson.Serialization.Serializers

/// <summary>
/// Serializer for F# lists.
/// </summary>
type FSharpListSerializer<'ElemType>() =
    inherit EnumerableSerializerBase<'ElemType list, 'ElemType>()

    override __.EnumerateItemsInSerializationOrder lst = List.toSeq lst

    // XXX: Using a mutable List because the AddItem member does not return a value,
    //      so it is not possible to accumulate into a list and reverse it in FinalizeResult
    override __.CreateAccumulator() = List<'ElemType>() |> box

    override __.AddItem (accumulator, item) =
        match accumulator with
        | :? List<'ElemType> as lst -> lst.Add item
        | _ -> failwithf "Expected accumulator to be a list, but got %A" accumulator

    override __.FinalizeResult accumulator =
        match accumulator with
        | :? List<'ElemType> as lst -> List.ofSeq lst
        | _ -> failwithf "Expected accumulator to be a list, but got %A" accumulator
