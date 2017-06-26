# Cake.Dotfuscator

只在 **Dotfuscator Professional Edition 4.9** 上测试通过。  

暂不支持 nuget.

### 使用说明
将生成的 Cake.Dotfuscator.dll 拷贝到 tools/Addins/  

```cake
#r tools/Addins/Cake.Dotfuscator.dll
```

### Example usage:

```Cake
Task("Dotfuscator")
    .IsDependentOn("Build")
    .WithCriteria(isRunningOnWindows)
    .Does(() => {
   
         var files = new string[] 
        { 
            "a.exe", 
            "b.exe",
            "c.dll",
            "d.dll",
            "e.dll",
            "f.dll"
        };

        Dotfuscator(files, new DotfuscatorSettings(){
            OutputDir = binDir,
            WorkingDirectory = binDir,
        });

});

```