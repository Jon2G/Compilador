.model small
.stack

.data
;=========================================================
;VARIABLES TEMPORALES PARA LAS OPERACIONES ARITMETICAS
;PRIMER OPERADOR
SGN1 db 0
NUM1 db 0,0,0,0
DEC1 db 0,0,0,0 
;SEGUNDO OPERADOR
SGN2 db 0
NUM2 db 0,0,0,0
DEC2 db 0,0,0,0

;AUXILIARES GLOBALES PARA LAS OPERACIONES
AUX  dw 0
;=========================================================
;VARIABLES TEMPORALES PARA LAS OPERACIONES LOGICAS
R_COMPARADOR  db 0
;=========================================================
.code
;INICIA PROCEDIMIENTO PRINCIPAL
begin proc FAR
MOV AX,@data
MOV DS,AX
;================>[LIMPIAR PANTALLA]<================
mov ax,0600h  ;ah 06(es un recorrido), al 00(pantalla completa)
mov bh,01Fh    ;fondo blanco(7), sobre azul(1)
mov cx,0000h  ;es la esquina superior izquierda reglon: columna
mov dx,184Fh ;es la esquina inferior derecha reglon: columna
int 10h  ;interrupcion que llama al BIOS
;================>[CODIGO GENERADO POR EL COMPILADOR]<================
;================[-CODIGO TRES DIRECCIONES-]================

;Parte Entera

MOV SGN1,01H
MOV NUM1[3h],2
MOV NUM1[2h],1
MOV NUM1[1h],0
MOV NUM1[0h],0

;Parte Decimal
MOV DEC1[0h],5
MOV DEC1[1h],5
MOV DEC1[2h],0
MOV DEC1[3h],0
;;================[-OPERACION LOGICA-]================

;Parte Entera

MOV SGN2,01H
MOV NUM2[3h],5
MOV NUM2[2h],0
MOV NUM2[1h],0
MOV NUM2[0h],0

;Parte Decimal
MOV DEC2[0h],5
MOV DEC2[1h],0
MOV DEC2[2h],0
MOV DEC2[3h],0


CALL MAYOR_QUE
MOV AL,R_COMPARADOR 

 
XOR AX,AX
INT 16H
;
MOV AH,4CH
INT 21H
RET

IGUAL PROC NEAR

    MOV AL,SGN1
    MOV AH,SGN2
    CMP AL,AH
    JE signo_igual_igualdad 
    
    MOV R_COMPARADOR,0H
    JMP salir_igualdad               
    
signo_igual_igualdad:    
    MOV SI,0FFFFH
    ;  
    siguiente_igualdad:
        INC SI 
        CMP SI,07H
        JA iguales
        
        MOV AL,NUM1[SI]
        MOV AH,NUM2[SI]
        CMP AL,AH
        JE siguiente_igualdad
        JNE diferente
    ;     
    
diferente:
    MOV R_COMPARADOR,0H
    JMP salir_igualdad
iguales:
    MOV R_COMPARADOR,1H
    
salir_igualdad:    
    RET    
IGUAL ENDP


MAYOR_QUE PROC NEAR

    MOV AL,SGN1
    MOV AH,SGN2
    CMP AL,AH
    JE signo_igual_mayorque
    JL menor_que_exit
    JC mayor_que_exit
    
    MOV R_COMPARADOR,0H
    JMP salir_mayorque               
    
signo_igual_mayorque:    
    MOV SI,0FFFFH
    ;  
    siguiente_mayorque:
        INC SI 
        CMP SI,07H
        JA iguales
        
        MOV AL,NUM1[SI]
        MOV AH,NUM2[SI]
        CMP AL,AH
        JA  mayor_que_exit    
        JL  menor_que_exit
        JE  siguiente_mayorque
        JMP siguiente_mayorque
    ;     
    
menor_que_exit:
    MOV R_COMPARADOR,0H
    JMP salir_igualdad
mayor_que_exit:
    MOV R_COMPARADOR,1H
    
salir_mayorque:    
    RET    
MAYOR_QUE ENDP

;==============>[FIN CODIGO GENERADO POR EL COMPILADOR]<==============

end begin


