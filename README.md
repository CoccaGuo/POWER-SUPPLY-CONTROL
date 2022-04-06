# Power Supply Controller(PSC) & PSC script generator

Power Supply Controller(PSC) is a tool for controlling the workflow of changing current or voltage.
Instead of spending much time on changing the current or voltage very often, this software allows the user to write their commands in a script and execute them consequently.
	
> The Controller is Wanptek WPS3010H (30V10A) with a special USB connector which may be added according to the request.

> The Controller was bought from Taobao.

![](readme.assets/demo.jpg)
	
## The script

The script show look like this:
	
```
	# 一个简单的电源脚本示意 a simple demo script
	# 使用PSC加载该脚本运行 use PSC to load it
	# 使用井号注释 use # to comment
	# author: CoccaGuo 
	# date: 2022.3.30
	# Comment should begin from the first line
	# 注释应另起一行书写
	# 识别标识符 不要删除下面这行字符  keep the line below as the first line
	
	PSC SCRIPT
	
	# 程序名 script name
	FUNC demo
	
	# 设置电压 set voltage
	SET VOLT 5.00
	# 设置电流 set current
	SET CURR 1.00
	# 开始输出 open output
	SET OUTPUT 1
	# 等待秒数 wait some seconds
	WAIT 1200
	
	# 关闭输出 close output
	SET OUTPUT 0
	# 脚本结束
	END
```

You can also use the generator to generate scripts. A generated one should look like this:
	
```
	
	# Automatic power supply script generated on 2022/4/6 14:24:52. Check before use.
	PSC SCRIPT
	
	func count_down
	# Functional test and preparation. DO NOT CHANGE.
	set volt 1
	set curr 0.05
	set output 1
	wait 5
	set output 1
	wait 5
	# test over.
	
	# loop begin.
	
	#loop #0
	set volt 1.000
	set curr 0.050
	wait 5
	
	#loop #1
	set volt 0.900
	set curr 0.050
	wait 5
	
	#loop #2
	set volt 0.800
	set curr 0.050
	wait 5
	
	#loop #3
	set volt 0.700
	set curr 0.050
	wait 5
	
	#loop #4
	set volt 0.600
	set curr 0.050
	wait 5
	
	#loop #5
	set volt 0.500
	set curr 0.050
	wait 5
	
	#loop #6
	set volt 0.400
	set curr 0.050
	wait 5
	
	#loop #7
	set volt 0.300
	set curr 0.050
	wait 5
	
	#loop #8
	set volt 0.200
	set curr 0.050
	wait 5
	
	#loop #9
	set volt 0.100
	set curr 0.050
	wait 5
	
	set volt 0
	set curr 0.05
	wait 5
	
	# End loop. Stopping power supply.
	
	set output 0
	wait 5
	set volt 0
	set curr 0
	set output 0
	end
	
	# End of the script.
	# Powered by Coccaguo. Version 0.1

```