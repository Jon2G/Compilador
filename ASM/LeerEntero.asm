.model small
.stack

.data
;=========================================================

SGN1 db 0
NUM1 db 0,0,0,0
DEC1 db 0,0,0,0

OVERFLOW DB 'STACK OVERFLOW!','$'
NUMERO_INVALIDO DB 'NUMERO INVALIDO!','$'
EXCEPCION_NO_CONTROLADA DB 'EXCEPCION NO CONTROLADA:','$'
AUXILIAR DB 0 
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
;================>[MOVER CURSOR A 0]<================
XOR DX,DX; dl Columna,dh fila
MOV bh,0 ;PAGINA
 mov ah, 02h ;mover cursor
int  10h; ;servicio de video 

LEA DI,SGN1
CALL LEER_ENTERO

MOV AH,4CH
INT 21H

RET
;================>[CODIGO GENERADO POR EL COMPILADOR]<================

LEER_ENTERO PROC NEAR 
    XOR CX,CX ;CUENTA EL NUMERO DE ENTEROS
    MOV SI,DI
    MOV AH,01H
    INT 21H
              
    MOV AH,'-'          
    CMP AL,AH    
    JE lectura_entero_negativo 
    INC DI  
    INC CX
    JMP validar_numero_entero
lectura_entero_negativo:
    MOV AL,0FFH 
    MOV [DI],AL   
    
    ;NUMEROS
    lectura_numeros:
    INC CX
    INC DI 
    CMP CX,06H 
    JAE overflow_numerico 
    
    MOV AH,01H
    INT 21H       
    
    CMP AL,0DH
    JE fin_lectura_entero 
    CMP AL,2EH
    JE lectura_numeros_decimales 
    validar_numero_entero:
    CMP AL,30H
    JL numero_invalido_enteros
    CMP AL,39H
    JA numero_invalido_enteros   
    
    SUB AL,30H
    MOV [DI],AL
    JMP lectura_numeros
    
    lectura_numeros_decimales:
    MOV DI,SI
    ADD DI,05H
    
    lectura_numero_decimal:
    CMP AUXILIAR,09H 
    JA overflow_numerico
    
    MOV AH,01H
    INT 21H
    
    CMP AL,0DH
    JE fin_lectura_entero
    CMP AL,30H
    JL numero_invalido_enteros
    CMP AL,39H
    JA numero_invalido_enteros   
       
    SUB AL,30H
    MOV [DI],AL
    
    INC AUXILIAR
    INC DI 
       
   JMP lectura_numero_decimal
      
fin_lectura_entero:
;AJUSTAR LA PARTE DECIMAL
DEC CX
CMP CX,04H
JL  ajuste_entero_necesario
JMP sin_ajuste_entero
ajuste_entero_necesario:  

MOV AL,CL
MOV AH,04H
SUB AH,AL
XOR DX,DX
MOV DL,AH

MOV DI,SI
INC DI
MOV BX,DI

DEC CX
siguiente_ajuste_entero:
MOV DI,BX
ADD DI,CX

MOV AL,[DI]
MOV AH,0H
MOV [DI],AH
ADD DI,DX
MOV [DI],AL


DEC CX
JNS siguiente_ajuste_entero

sin_ajuste_entero:
RET    
overflow_numerico: 
LEA DI,OVERFLOW  
CALL EXCEPCION  
numero_invalido_enteros:
LEA DI,NUMERO_INVALIDO
CALL EXCEPCION
               
RET               
LEER_ENTERO ENDP
EXCEPCION PROC NEAR

MOV AH, 06h    ; Scroll up function
XOR AL, AL     ; Clear entire screen
XOR CX, CX     ; Upper left corner CH=row, CL=column
MOV DX, 184FH  ; lower right corner DH=row, DL=column 
MOV BH, 4Eh    ; YellowOnBlue
INT 10H

MOV DL,1AH
MOV DH,0AH
MOV bh,0 ;PAGINA
mov ah, 02h ;mover cursor
int  10h; ;servicio de video 

 
LEA DX,EXCEPCION_NO_CONTROLADA
MOV AH,09H
INT 21H
 
MOV DL,1AH
MOV DH,0CH
MOV bh,0 ;PAGINA
mov ah, 02h ;mover cursor
int  10h; ;servicio de video 
 
MOV DX,DI
MOV AH,09H
INT 21H         

XOR AX,AX
INT 16H

MOV AH,4CH
INT 21H
              
EXCEPCION ENDP

  
end begin
