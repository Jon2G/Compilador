mount C: "C:\Program Files (x86)\GUI Turbo Assembler\BIN"
            C:
            tasm.exe VOMRTKJ.asm >gtasm.log
            tlink.exe /3 /x /v VOMRTKJ >gtlink.log
            exit