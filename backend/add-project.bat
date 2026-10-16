@echo off
rem همه csproj های زیر hosts و modules را به solution اضافه می‌کند
for /r %%f in (*.csproj) do dotnet sln IKCO.ProductionFeasibility.sln add "%%f"
