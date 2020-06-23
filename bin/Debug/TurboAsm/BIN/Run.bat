mount C: "C:\Program Files (x86)\GUI Turbo Assembler\BIN"
            C:
            tasm.exe GSBMVYW.asm >gtasm.log
            tlink.exe /3 /x /v GSBMVYW >gtlink.log
            exit