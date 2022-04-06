# Power Supply Controller(PSC) & PSC script generator

Power Supply Controller(PSC) is a tool for controlling the workflow of changing current or voltage.
Instead of spending much time on changing the current or voltage very often, this software allows the user to write their commands in a script and execute them consequently.
	
> The Controller is Wanptek WPS3010H (30V10A) with a special USB connector which may be added according to the request.

> The Controller was bought from Taobao.

![](readme.assets/demo.jpg)

## Releases

https://github.com/CoccaGuo/POWER-SUPPLY-CONTROL/releases

VER 2.0.0

- ������ͼ����
- ������UDP���������ܣ����Դ�������������Ϣ
- [debug]�����˶˿�ѡ����
- [debug]�����������ⲿ������ƹ���

VER 2.1.0

- �����˶˿�ѡ����

- �����������ⲿ������ƹ���

- �趨��UDPͨ��Ӧ��֮8λbyte�ṹ

  2.1.1

  - �޸��˳�ʼ�οڲ���ʾ����
    2.1.2
  - �޸�����ͣ��ʣ��ʱ�仹�ڼ��ٵ�bug
    2.1.3
  - �޸����޷�ʵ�ʿ��Ƶ���Դ��bug
  - �޸��˿�ʼ�������ʾ����COM1��bug

�⽫��2.0�汾�����һ����Ҫ���£����Ҳ�ǳ���˺����Ҫά���汾����Ϊ�书���Ѿ��걸��
��һ���ȶ��汾��v1.5.1��v1.4.0	

## The script

The script show look like this:
	
```
	# һ���򵥵ĵ�Դ�ű�ʾ�� a simple demo script
	# ʹ��PSC���ظýű����� use PSC to load it
	# ʹ�þ���ע�� use # to comment
	# author: CoccaGuo 
	# date: 2022.3.30
	# Comment should begin from the first line
	# ע��Ӧ����һ����д
	# ʶ���ʶ�� ��Ҫɾ�����������ַ�  keep the line below as the first line
	
	PSC SCRIPT
	
	# ������ script name
	FUNC demo
	
	# ���õ�ѹ set voltage
	SET VOLT 5.00
	# ���õ��� set current
	SET CURR 1.00
	# ��ʼ��� open output
	SET OUTPUT 1
	# �ȴ����� wait some seconds
	WAIT 1200
	
	# �ر���� close output
	SET OUTPUT 0
	# �ű�����
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