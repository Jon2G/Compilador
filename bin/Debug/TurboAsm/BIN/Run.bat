mount C: "C:\Program Files (x86)\GUI Turbo Assembler\BIN"
            C:
            tasm.exe QWASTLN.asm >gtasm.log
            tlink.exe /3 /x /v QWASTLN >gtlink.log
            exit