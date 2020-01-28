
mount C: "C:\Program Files (x86)\GUI Turbo Assembler\BIN"
C:
tasm.exe mycode.asm >gtasm.log
tlink.exe /3 /x /v mycode >gtlink.log

mycode.exe
pause
exit
