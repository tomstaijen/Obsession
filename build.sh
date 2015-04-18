#!/bin/bash

mozroots --import --sync

certmgr -ssl -m https://go.microsoft.com
certmgr -ssl -m https://nugetgallery.blob.core.windows.net
certmgr -ssl -m https://nuget.org

mono .nuget/NuGet.exe restore Obsession.sln

xbuild Obsession.sln
