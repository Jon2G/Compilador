.model small
.stack
.386
.data
;=========================================================
;VARIABLES TEMPORALES PARA LAS OPERACIONES ARITMETICAS
;PRIMER OPERADOR
SGN1 db 0
NUM1 db 0,0,0,0,0,0,0,0,0
DEC1 db 0,0,0,0,0,0,0,0,0 
;SEGUNDO OPERADOR
SGN2 db 0
NUM2 db 0,0,0,0,0,0,0,0,0
DEC2 db 0,0,0,0,0,0,0,0,0
;VARIABLES DE RESULTADOS GLOBAL
SIGNOT     db 0
ENTEROST   db 0,0,0,0,0,0,0,0,0
DECIMALEST db 0,0,0,0,0,0,0,0,0
;AUXILIARES GLOBALES PARA LAS OPERACIONES
TEMP db 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
AUX  dw 0
POSICION_DIV dw 0
OVERFLOWDIV dw 0
DIVISON dw 0,0,0
;=========================================================
;VARIABLES PARA LA LEECTURA DE CADENAS
CADENA DW 00
LONGUITUD_CADENA DW 0
BLOQUE_ACTUAL DB 0
ULTIMO_SEGMENTO DW 0
ERROR DB 'ERROR','$'
REALLOC DB 10,13,'REALLOC',10,13,'$'
ERROR_ALLOC_CADENA DB 10,13,'OCURRIO UN ERROR RESERVANDO MEMORIA PARA LA CADENA',10,13,'$'
FIN_CADENA DB '$'
;=========================================================
;VARIABLES TEMPORALES PARA LAS OPERACIONES LOGICAS
R_COMPARADOR  db 0
CAD_TEMP1  dw 0
CAD_TEMP2  dw 0
;=========================================================
NUMERO_INVALIDO DB 'NUMERO INVALIDO!','$'
AUXILIAR  DB 0
OVERFLOW DB 'STACK OVERFLOW!','$'
EXCEPCION_NO_CONTROLADA DB 'EXCEPCION NO CONTROLADA:','$'
;VARIABLES DEPENDIENTES DE LA PROGRAMACION

;=======>[Importe-Decimal]
SGN_Importe db 0
NUM_Importe db 0,0,0,0,0,0,0,0,0
DEC_Importe db 0,0,0,0,0,0,0,0,0 

;=======>[DescuentoPCT-Entero]
SGN_DescuentoPCT db 0
NUM_DescuentoPCT db 0,0,0,0,0,0,0,0,0
DEC_DescuentoPCT db 0,0,0,0,0,0,0,0,0 

;=======>[Descuento-Entero]
SGN_Descuento db 0
NUM_Descuento db 0,0,0,0,0,0,0,0,0
DEC_Descuento db 0,0,0,0,0,0,0,0,0 

;=======>[AUTOVAR7eba1c1eaa99436b8dd18602baacf083-Cadena]
AUTOVAR7eba1c1eaa99436b8dd18602baacf083 DW 00
LONGUITUD_AUTOVAR7eba1c1eaa99436b8dd18602baacf083 DW 0

;=======>[AUTOVARd91d482571944e7298d3f459f1a2a507-Cadena]
AUTOVARd91d482571944e7298d3f459f1a2a507 DW 00
LONGUITUD_AUTOVARd91d482571944e7298d3f459f1a2a507 DW 0

;=======>[AUTOVAR3a90baa00e5644f4af11fb5f28c35d73-Cadena]
AUTOVAR3a90baa00e5644f4af11fb5f28c35d73 DW 00
LONGUITUD_AUTOVAR3a90baa00e5644f4af11fb5f28c35d73 DW 0

;=======>[AUTOVARe1ade7f87fed4452a21d456562e5ee3f-Cadena]
AUTOVARe1ade7f87fed4452a21d456562e5ee3f DW 00
LONGUITUD_AUTOVARe1ade7f87fed4452a21d456562e5ee3f DW 0

;=======>[AUTOVAR6646e9ee9eca41948cef23196a95d479-Cadena]
AUTOVAR6646e9ee9eca41948cef23196a95d479 DW 00
LONGUITUD_AUTOVAR6646e9ee9eca41948cef23196a95d479 DW 0

;=======>[AUTOVARf2d3511d43644e0ab558d8eebe73490b-Cadena]
AUTOVARf2d3511d43644e0ab558d8eebe73490b DW 00
LONGUITUD_AUTOVARf2d3511d43644e0ab558d8eebe73490b DW 0

;=======>[AUTOVAR99e92321499d4e0d9490b189ce66b82e-Cadena]
AUTOVAR99e92321499d4e0d9490b189ce66b82e DW 00
LONGUITUD_AUTOVAR99e92321499d4e0d9490b189ce66b82e DW 0

;=======>[AUTOVAR2e242ef3b3d4403da1367542a2fa3aa4-Cadena]
AUTOVAR2e242ef3b3d4403da1367542a2fa3aa4 DW 00
LONGUITUD_AUTOVAR2e242ef3b3d4403da1367542a2fa3aa4 DW 0

;=======>[R_COMPARADOR_1-BitLogico]
R_COMPARADOR_1 DB 0

;=======>[SIGNOT1-Decimal]
SGN_SIGNOT1 db 0
NUM_SIGNOT1 db 0,0,0,0,0,0,0,0,0
DEC_SIGNOT1 db 0,0,0,0,0,0,0,0,0 

;=======>[ENTEROST1-Decimal]
SGN_ENTEROST1 db 0
NUM_ENTEROST1 db 0,0,0,0,0,0,0,0,0
DEC_ENTEROST1 db 0,0,0,0,0,0,0,0,0 

;=======>[DECIMALEST1-Decimal]
SGN_DECIMALEST1 db 0
NUM_DECIMALEST1 db 0,0,0,0,0,0,0,0,0
DEC_DECIMALEST1 db 0,0,0,0,0,0,0,0,0 

;=======>[AUTOVAR8e4ce4a5672442fd907781e6479a95e9-Cadena]
AUTOVAR8e4ce4a5672442fd907781e6479a95e9 DW 00
LONGUITUD_AUTOVAR8e4ce4a5672442fd907781e6479a95e9 DW 0

;=======>[AUTOVAR65e9de3768674ade9714c2f2e445cf22-Cadena]
AUTOVAR65e9de3768674ade9714c2f2e445cf22 DW 00
LONGUITUD_AUTOVAR65e9de3768674ade9714c2f2e445cf22 DW 0
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
;================>[CODIGO GENERADO POR EL COMPILADOR]<================
;================[-ALOGAR CADENA-]================
MOV BLOQUE_ACTUAL,13D
CALL ALOGAR_CADENA
MOV SI,CADENA
;=================[ASIGNAR VALOR A LA CADENA]=================
MOV DL,0A8H
MOV [SI],DL
INC SI
MOV DL,'I'
MOV [SI],DL
INC SI
MOV DL,'n'
MOV [SI],DL
INC SI
MOV DL,'g'
MOV [SI],DL
INC SI
MOV DL,'r'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,'s'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,' '
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,'l'
MOV [SI],DL
INC SI
MOV DL,' '
MOV [SI],DL
INC SI
MOV DL,'i'
MOV [SI],DL
INC SI
MOV DL,'m'
MOV [SI],DL
INC SI
MOV DL,'p'
MOV [SI],DL
INC SI
MOV DL,'o'
MOV [SI],DL
INC SI
MOV DL,'r'
MOV [SI],DL
INC SI
MOV DL,'t'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,' '
MOV [SI],DL
INC SI
MOV DL,'d'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,' '
MOV [SI],DL
INC SI
MOV DL,'l'
MOV [SI],DL
INC SI
MOV DL,'a'
MOV [SI],DL
INC SI
MOV DL,' '
MOV [SI],DL
INC SI
MOV DL,'c'
MOV [SI],DL
INC SI
MOV DL,'o'
MOV [SI],DL
INC SI
MOV DL,'m'
MOV [SI],DL
INC SI
MOV DL,'p'
MOV [SI],DL
INC SI
MOV DL,'r'
MOV [SI],DL
INC SI
MOV DL,'a'
MOV [SI],DL
INC SI
MOV DL,':'
MOV [SI],DL
INC SI
MOV DL,FIN_CADENA
MOV [SI],DL
MOV DX,CADENA
MOV AUTOVAR7eba1c1eaa99436b8dd18602baacf083,DX
MOV DX,AUTOVAR7eba1c1eaa99436b8dd18602baacf083
CALL IMPRIME_CADENA
LEA DI,SGN_Importe
CALL LEER_DECIMAL
;================[-ALOGAR CADENA-]================
MOV BLOQUE_ACTUAL,13D
CALL ALOGAR_CADENA
MOV SI,CADENA
;=================[ASIGNAR VALOR A LA CADENA]=================
MOV DL,10
MOV [SI],DL
INC SI
MOV DL,13
MOV [SI],DL
INC SI
MOV DL,'I'
MOV [SI],DL
INC SI
MOV DL,'n'
MOV [SI],DL
INC SI
MOV DL,'g'
MOV [SI],DL
INC SI
MOV DL,'r'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,'s'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,' '
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,'l'
MOV [SI],DL
INC SI
MOV DL,' '
MOV [SI],DL
INC SI
MOV DL,'d'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,'s'
MOV [SI],DL
INC SI
MOV DL,'c'
MOV [SI],DL
INC SI
MOV DL,'u'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,'n'
MOV [SI],DL
INC SI
MOV DL,'t'
MOV [SI],DL
INC SI
MOV DL,'o'
MOV [SI],DL
INC SI
MOV DL,' '
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,'n'
MOV [SI],DL
INC SI
MOV DL,' '
MOV [SI],DL
INC SI
MOV DL,'p'
MOV [SI],DL
INC SI
MOV DL,'o'
MOV [SI],DL
INC SI
MOV DL,'r'
MOV [SI],DL
INC SI
MOV DL,'c'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,'n'
MOV [SI],DL
INC SI
MOV DL,'t'
MOV [SI],DL
INC SI
MOV DL,'a'
MOV [SI],DL
INC SI
MOV DL,'j'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,':'
MOV [SI],DL
INC SI
MOV DL,FIN_CADENA
MOV [SI],DL
MOV DX,CADENA
MOV AUTOVARd91d482571944e7298d3f459f1a2a507,DX
MOV DX,AUTOVARd91d482571944e7298d3f459f1a2a507
CALL IMPRIME_CADENA
LEA DI,SGN_DescuentoPCT
CALL LEER_ENTERO
;================[-CODIGO TRES DIRECCIONES-]================
;=========[T1 = DescuentoPCT/100]=========

LEA DX,SGN_DescuentoPCT
CALL USAR_N1  ;vamos a utilizar T_DescuentoPCT

;Parte Entera

 MOV SGN2,1H
MOV NUM2[8h],0
MOV NUM2[7h],0
MOV NUM2[6h],1
MOV NUM2[5h],0
MOV NUM2[4h],0
MOV NUM2[3h],0
MOV NUM2[2h],0
MOV NUM2[1h],0
MOV NUM2[0h],0

;Parte Decimal
MOV DEC2[0h],0
MOV DEC2[1h],0
MOV DEC2[2h],0
MOV DEC2[3h],0
MOV DEC2[4h],0
MOV DEC2[5h],0
MOV DEC2[6h],0
MOV DEC2[7h],0
MOV DEC2[8h],0
CALL DIVIDE
LEA DX,SGN_SIGNOT1
CALL SALVAR_TEMPORAL

LEA DX,SGN_DescuentoPCT
CALL SALVAR_TEMPORAL
;================[-CODIGO TRES DIRECCIONES-]================
;=========[T1 = Importe*DescuentoPCT]=========

LEA DX,SGN_Importe
CALL USAR_N1  ;vamos a utilizar T_Importe
LEA DX,SGN_DescuentoPCT
CALL USAR_N2  ;vamos a utilizar T_DescuentoPCT
CALL MULTIPLICA
LEA DX,SGN_SIGNOT1
CALL SALVAR_TEMPORAL

LEA DX,SGN_Descuento
CALL SALVAR_TEMPORAL
;================>[LIMPIAR PANTALLLA]<================
mov ax,0600h  
 mov bh,01Fh 
 mov cx,0000h 
 mov dx,184Fh 
int 10h 
;================>[MOVER CURSOR A 0]<================
XOR DX,DX
MOV bh,0 
 mov ah, 02h 
int  10h
;================[-ALOGAR CADENA-]================
MOV BLOQUE_ACTUAL,15D
CALL ALOGAR_CADENA
MOV SI,CADENA
;=================[ASIGNAR VALOR A LA CADENA]=================
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,10
MOV [SI],DL
INC SI
MOV DL,13
MOV [SI],DL
INC SI
MOV DL,FIN_CADENA
MOV [SI],DL
MOV DX,CADENA
MOV AUTOVAR3a90baa00e5644f4af11fb5f28c35d73,DX
MOV DX,AUTOVAR3a90baa00e5644f4af11fb5f28c35d73
CALL IMPRIME_CADENA
;================[-ALOGAR CADENA-]================
MOV BLOQUE_ACTUAL,12D
CALL ALOGAR_CADENA
MOV SI,CADENA
;=================[ASIGNAR VALOR A LA CADENA]=================
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'>'
MOV [SI],DL
INC SI
MOV DL,' '
MOV [SI],DL
INC SI
MOV DL,'D'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,'s'
MOV [SI],DL
INC SI
MOV DL,'c'
MOV [SI],DL
INC SI
MOV DL,'u'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,'n'
MOV [SI],DL
INC SI
MOV DL,'t'
MOV [SI],DL
INC SI
MOV DL,'o'
MOV [SI],DL
INC SI
MOV DL,' '
MOV [SI],DL
INC SI
MOV DL,'%'
MOV [SI],DL
INC SI
MOV DL,FIN_CADENA
MOV [SI],DL
MOV DX,CADENA
MOV AUTOVARe1ade7f87fed4452a21d456562e5ee3f,DX
MOV DX,AUTOVARe1ade7f87fed4452a21d456562e5ee3f
CALL IMPRIME_CADENA
LEA DI,SGN_DescuentoPCT
CALL MOSTRAR_RESULTADO
;================[-ALOGAR CADENA-]================
MOV BLOQUE_ACTUAL,12D
CALL ALOGAR_CADENA
MOV SI,CADENA
;=================[ASIGNAR VALOR A LA CADENA]=================
MOV DL,10
MOV [SI],DL
INC SI
MOV DL,13
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'>'
MOV [SI],DL
INC SI
MOV DL,' '
MOV [SI],DL
INC SI
MOV DL,'D'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,'s'
MOV [SI],DL
INC SI
MOV DL,'c'
MOV [SI],DL
INC SI
MOV DL,'u'
MOV [SI],DL
INC SI
MOV DL,'e'
MOV [SI],DL
INC SI
MOV DL,'n'
MOV [SI],DL
INC SI
MOV DL,'t'
MOV [SI],DL
INC SI
MOV DL,'o'
MOV [SI],DL
INC SI
MOV DL,' '
MOV [SI],DL
INC SI
MOV DL,'t'
MOV [SI],DL
INC SI
MOV DL,'o'
MOV [SI],DL
INC SI
MOV DL,'t'
MOV [SI],DL
INC SI
MOV DL,'a'
MOV [SI],DL
INC SI
MOV DL,'l'
MOV [SI],DL
INC SI
MOV DL,' '
MOV [SI],DL
INC SI
MOV DL,015H
MOV [SI],DL
INC SI
MOV DL,FIN_CADENA
MOV [SI],DL
MOV DX,CADENA
MOV AUTOVAR6646e9ee9eca41948cef23196a95d479,DX
MOV DX,AUTOVAR6646e9ee9eca41948cef23196a95d479
CALL IMPRIME_CADENA
LEA DI,SGN_Descuento
CALL MOSTRAR_RESULTADO
;================[-ALOGAR CADENA-]================
MOV BLOQUE_ACTUAL,15D
CALL ALOGAR_CADENA
MOV SI,CADENA
;=================[ASIGNAR VALOR A LA CADENA]=================
MOV DL,10
MOV [SI],DL
INC SI
MOV DL,13
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,10
MOV [SI],DL
INC SI
MOV DL,13
MOV [SI],DL
INC SI
MOV DL,FIN_CADENA
MOV [SI],DL
MOV DX,CADENA
MOV AUTOVARf2d3511d43644e0ab558d8eebe73490b,DX
MOV DX,AUTOVARf2d3511d43644e0ab558d8eebe73490b
CALL IMPRIME_CADENA
;================[-ALOGAR CADENA-]================
MOV BLOQUE_ACTUAL,12D
CALL ALOGAR_CADENA
MOV SI,CADENA
;=================[ASIGNAR VALOR A LA CADENA]=================
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'>'
MOV [SI],DL
INC SI
MOV DL,' '
MOV [SI],DL
INC SI
MOV DL,'S'
MOV [SI],DL
INC SI
MOV DL,'u'
MOV [SI],DL
INC SI
MOV DL,'b'
MOV [SI],DL
INC SI
MOV DL,'t'
MOV [SI],DL
INC SI
MOV DL,'o'
MOV [SI],DL
INC SI
MOV DL,'t'
MOV [SI],DL
INC SI
MOV DL,'a'
MOV [SI],DL
INC SI
MOV DL,'l'
MOV [SI],DL
INC SI
MOV DL,' '
MOV [SI],DL
INC SI
MOV DL,015H
MOV [SI],DL
INC SI
MOV DL,FIN_CADENA
MOV [SI],DL
MOV DX,CADENA
MOV AUTOVAR99e92321499d4e0d9490b189ce66b82e,DX
MOV DX,AUTOVAR99e92321499d4e0d9490b189ce66b82e
CALL IMPRIME_CADENA
LEA DI,SGN_Importe
CALL MOSTRAR_RESULTADO
;================[-CODIGO TRES DIRECCIONES-]================
;=========[T1 = Importe-Descuento]=========

LEA DX,SGN_Importe
CALL USAR_N1  ;vamos a utilizar T_Importe
LEA DX,SGN_Descuento
CALL USAR_N2  ;vamos a utilizar T_Descuento
CALL RESTA
LEA DX,SGN_SIGNOT1
CALL SALVAR_TEMPORAL

LEA DX,SGN_Importe
CALL SALVAR_TEMPORAL
;================[-ALOGAR CADENA-]================
MOV BLOQUE_ACTUAL,12D
CALL ALOGAR_CADENA
MOV SI,CADENA
;=================[ASIGNAR VALOR A LA CADENA]=================
MOV DL,10
MOV [SI],DL
INC SI
MOV DL,13
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'>'
MOV [SI],DL
INC SI
MOV DL,' '
MOV [SI],DL
INC SI
MOV DL,'T'
MOV [SI],DL
INC SI
MOV DL,'o'
MOV [SI],DL
INC SI
MOV DL,'t'
MOV [SI],DL
INC SI
MOV DL,'a'
MOV [SI],DL
INC SI
MOV DL,'l'
MOV [SI],DL
INC SI
MOV DL,' '
MOV [SI],DL
INC SI
MOV DL,015H
MOV [SI],DL
INC SI
MOV DL,FIN_CADENA
MOV [SI],DL
MOV DX,CADENA
MOV AUTOVAR2e242ef3b3d4403da1367542a2fa3aa4,DX
MOV DX,AUTOVAR2e242ef3b3d4403da1367542a2fa3aa4
CALL IMPRIME_CADENA
;;================[-OPERACION LOGICA-]================
;T1 = Importe<=0
;=========[T1 = Importe<=0]=========

LEA DX,SGN_Importe
CALL USAR_N1  ;vamos a utilizar T_Importe

;Parte Entera

 MOV SGN2,1H
MOV NUM2[8h],0
MOV NUM2[7h],0
MOV NUM2[6h],0
MOV NUM2[5h],0
MOV NUM2[4h],0
MOV NUM2[3h],0
MOV NUM2[2h],0
MOV NUM2[1h],0
MOV NUM2[0h],0

;Parte Decimal
MOV DEC2[0h],0
MOV DEC2[1h],0
MOV DEC2[2h],0
MOV DEC2[3h],0
MOV DEC2[4h],0
MOV DEC2[5h],0
MOV DEC2[6h],0
MOV DEC2[7h],0
MOV DEC2[8h],0
CALL MENOR_QUE_IGUAL
MOV AL,R_COMPARADOR
MOV R_COMPARADOR_1,AL

MOV AL,1H
CMP R_COMPARADOR,AL
JE IF_VERDADERO_1
JMP INICIO_SINO_1
IF_VERDADERO_1:
;ACCIONES VERDADERAS :)
;================[-ALOGAR CADENA-]================
MOV BLOQUE_ACTUAL,11D
CALL ALOGAR_CADENA
MOV SI,CADENA
;=================[ASIGNAR VALOR A LA CADENA]=================
MOV DL,'0'
MOV [SI],DL
INC SI
MOV DL,'.'
MOV [SI],DL
INC SI
MOV DL,'0'
MOV [SI],DL
INC SI
MOV DL,'0'
MOV [SI],DL
INC SI
MOV DL,' '
MOV [SI],DL
INC SI
MOV DL,'('
MOV [SI],DL
INC SI
MOV DL,'G'
MOV [SI],DL
INC SI
MOV DL,'R'
MOV [SI],DL
INC SI
MOV DL,'A'
MOV [SI],DL
INC SI
MOV DL,'T'
MOV [SI],DL
INC SI
MOV DL,'I'
MOV [SI],DL
INC SI
MOV DL,'S'
MOV [SI],DL
INC SI
MOV DL,')'
MOV [SI],DL
INC SI
MOV DL,FIN_CADENA
MOV [SI],DL
MOV DX,CADENA
MOV AUTOVAR8e4ce4a5672442fd907781e6479a95e9,DX
MOV DX,AUTOVAR8e4ce4a5672442fd907781e6479a95e9
CALL IMPRIME_CADENA
IF_FALSO_1:
;ACCIONES FALSAS :(
JMP FIN_SINO1
INICIO_SINO_1:
LEA DI,SGN_Importe
CALL MOSTRAR_RESULTADO
FIN_SINO1:
;================[-ALOGAR CADENA-]================
MOV BLOQUE_ACTUAL,15D
CALL ALOGAR_CADENA
MOV SI,CADENA
;=================[ASIGNAR VALOR A LA CADENA]=================
MOV DL,10
MOV [SI],DL
INC SI
MOV DL,13
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'='
MOV [SI],DL
INC SI
MOV DL,'-'
MOV [SI],DL
INC SI
MOV DL,10
MOV [SI],DL
INC SI
MOV DL,13
MOV [SI],DL
INC SI
MOV DL,FIN_CADENA
MOV [SI],DL
MOV DX,CADENA
MOV AUTOVAR65e9de3768674ade9714c2f2e445cf22,DX
MOV DX,AUTOVAR65e9de3768674ade9714c2f2e445cf22
CALL IMPRIME_CADENA
;==============>[FIN CODIGO GENERADO POR EL COMPILADOR]<==============
XOR AX,AX
INT 16H
;================>[LIBERAR TODA LA MEMORIA DE CADENAS]<================
MOV BX,AUTOVAR7eba1c1eaa99436b8dd18602baacf083
CALL LIBERAR_CADENA
MOV BX,AUTOVARd91d482571944e7298d3f459f1a2a507
CALL LIBERAR_CADENA
MOV BX,AUTOVAR3a90baa00e5644f4af11fb5f28c35d73
CALL LIBERAR_CADENA
MOV BX,AUTOVARe1ade7f87fed4452a21d456562e5ee3f
CALL LIBERAR_CADENA
MOV BX,AUTOVAR6646e9ee9eca41948cef23196a95d479
CALL LIBERAR_CADENA
MOV BX,AUTOVARf2d3511d43644e0ab558d8eebe73490b
CALL LIBERAR_CADENA
MOV BX,AUTOVAR99e92321499d4e0d9490b189ce66b82e
CALL LIBERAR_CADENA
MOV BX,AUTOVAR2e242ef3b3d4403da1367542a2fa3aa4
CALL LIBERAR_CADENA
MOV BX,AUTOVAR8e4ce4a5672442fd907781e6479a95e9
CALL LIBERAR_CADENA
MOV BX,AUTOVAR65e9de3768674ade9714c2f2e445cf22
CALL LIBERAR_CADENA
MOV AH,4CH
INT 21H
RET

;==============>[PROCEDIMIENTOS INTERNOS GENERADOS POR EL COMPILADOR]<==============
;====================================[DIVIDE]====================================    
    DIVIDE PROC NEAR
        MOV DIVISON[0H],0
        MOV DIVISON[2H],0
        MOV DIVISON[4H],0
        
        MOV POSICION_DIV,0H     

        ;LOGICA DE SIGNOS :) BASTANTE SENCILLO AQUI
        MOV AL,SGN1
        MOV AH,SGN2
        IMUL AH
        PUSH AX ;SALVAR SIGNO

        ;PARTE_ENTERA
        divide_siguiente:
           
        CALL RESTA   
        MOV AL,SIGNOT
        CMP AL,0FFH
        JE parte_decimal
        
        ;  
            CMP POSICION_DIV,0H
            JE enteros_div  
            JA decimales_div_
                enteros_div:
                MOV SI,POSICION_DIV 
                ADD DIVISON[SI],01H
                  
                JMP continuar_div
                decimales_div_:
                MOV SI,POSICION_DIV
                ADD DIVISON[SI],01H  
                ;JMP continuar_div
                
            continuar_div:    
            MOV SIGNOT,0H 
            LEA DX,SIGNOT
            CALL USAR_N1 
        ;
        JMP divide_siguiente
        
        
        parte_decimal:   
        ;MULTIPLICAR RESUIDO POR 10    
        MOV SI,018D    
        
        copy_next_div: 
        MOV AL,SGN1[SI]
        MOV TEMP[SI],AL
        DEC SI
        JNS copy_next_div
        
        LEA DX,SIGNOT
        CALL LIMPIAR
        
        MOV SGN1,0H
        MOV NUM1[0],0
        MOV NUM1[1],0
        MOV NUM1[2],0
        MOV NUM1[3],0 
        MOV NUM1[4],0 
        MOV NUM1[5],0 
        MOV NUM1[6],0 
        MOV NUM1[7],1 
        MOV NUM1[8],0 
        
        MOV DEC1[0],0
        MOV DEC1[1],0
        MOV DEC1[2],0
        MOV DEC1[3],0
        MOV DEC1[4],0
        MOV DEC1[5],0
        MOV DEC1[6],0
        MOV DEC1[7],0
        MOV DEC1[8],0
        
        CALL MULTIPLICA
        
        LEA DX,TEMP
        CALL USAR_N2 
        
        LEA DX,SIGNOT
        CALL USAR_N1
        
        ADD POSICION_DIV,2H
        CMP POSICION_DIV,06H
        JAE fin_division
        ;;;;;;;;;;;;;;;;;;REVISAR QUE EL NUM1 NO SEA YA CERO :V
        MOV SI,018D ;Posicion del ultimo digito decimal ~  
        revisar_siguiente_div:
            MOV AL,ENTEROST[SI]
            CMP AL,0H
            JNE divide_siguiente
        DEC SI
        CMP SI,0H
        JE fin_division
        JNE revisar_siguiente_div 
        ;;;;;;;;;;;;;;;;;;
        
        JMP divide_siguiente
        fin_division:
        ;COPIAR RESULTADO A T
            ;LIMPIAR VARIABLES
            LEA DX,SIGNOT
            CALL LIMPIAR

            MOV AX,DIVISON[0H]
            AAM
            MOV ENTEROST[7H],AH
            MOV ENTEROST[8H],AL
            
            MOV AX,DIVISON[2H]
            AAM
            MOV DECIMALEST[0H],AL
            MOV DECIMALEST[1H],AH 
            MOV AX,DIVISON[4H]
            AAM
            ADD DECIMALEST[0H],AH 
            ADD DECIMALEST[1H],AL

           POP AX 
           MOV SIGNOT,AL  ;RECUPERAR SINGO
        RET 
        
    DIVIDE ENDP
;====================================[RESTA]====================================        
    RESTA PROC NEAR  
    ;LIMPIAR OPERADORES 
    LEA DX,SIGNOT
    CALL LIMPIAR               
    ;;ACOMODAR LOS OPERANDOS DE MODO QUE EL MAYOR QUEDE EN NUM1
    MOV SI,0H
    acomodar_siguiente:
        MOV AL,NUM1[SI]
        MOV AH,NUM2[SI]
        CMP AL,AH
        JL invertir_operandos
        JA resta_es_positivo  
        INC SI
        CMP SI,018D
        JE acomodo_terminado
        JMP acomodar_siguiente 

    resta_es_positivo:
        MOV SIGNOT,01H
    JMP acomodo_terminado
    
    invertir_operandos:
    MOV SIGNOT,0FFH
    MOV SI,017D   ;~MOV SI,7H     
    invertir_siguiente:
    MOV AL,NUM1[SI]
    MOV AH,NUM2[SI]
    
    MOV NUM1[SI],AH
    MOV NUM2[SI],AL
    DEC SI
    JS acomodo_terminado 
    JMP invertir_siguiente 
    
    acomodo_terminado:
    ;;   
            ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
            MOV SI,017D ;NUMERO MAS A LA IZQUIERDA             
        
                siguiente_restando:
            

                    MOV AL,NUM1[SI] ;OP_1 
                    MOV AH,NUM2[SI]
                    SUB AL,AH
                    ;;===
                        ADD AL,ENTEROST[SI]           
                        MOV ENTEROST[SI],AL
                        ;
                    ;;===
                    DEC SI
                    JNS siguiente_restando                      
                    

            ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;       
            
            ;Ajustar acarreos        
        
            MOV SI,017D  
        
            siguiente_acarreo_neg:
        
            MOV AL,ENTEROST[SI]
            CMP AL,0H 
            JS con_acarreo_neg
            JMP sin_acarreo_neg   
            ;;
            con_acarreo_neg: 
                NEG AL
                MOV AH,0AH
                SUB AH,AL
                MOV ENTEROST[SI],AH
                
                MOV AL,ENTEROST[SI-1]
                SUB AL,01H
                MOV ENTEROST[SI-1],AL
            ;;
            sin_acarreo_neg:  
            DEC SI
            JNS siguiente_acarreo_neg
            RET
    
        
    RESTA ENDP
;====================================[MULTIPLICA]====================================
    MULTIPLICA PROC NEAR
    ;LIMPIAR VARIABLES
    LEA DX,SIGNOT
    CALL LIMPIAR
    ;LOGICA DE SIGNOS :) bastante sencillo aqui
        MOV AL,SGN1
        MOV AH,SGN2
        IMUL AH
        MOV SIGNOT,AL 
        
            MOV AUX,0H  
            ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
            MOV SI,12D ;NUMERO MAS A LA IZQUIERDA             
        
                siguiente_multiplicando:
                MOV DI,012D ;NUMERO MAS A LA IZQUIERDA
                ;MOV CX,7H 
                    MOV CX,016D
                    
                SUB CX,AUX
            
                 multiplicar_operandos:
                    MOV AL,NUM2[SI] ;OP_1  
                    MOV AH,NUM1[DI]                 
                    MUL AH
                    AAM
                    ;;===
                    MOV BX,DI
                    MOV DI,CX
                
                        ;
                        MOV DX,DI
                        MOV DI,CX
                    
                        ADD AL,ENTEROST[DI]           
                        MOV ENTEROST[DI],AL

                   
                        ADD AH,ENTEROST[DI-1]
                        MOV ENTEROST[DI-1],AH  
                    
                        MOV DI,DX
                        ;
                
                    ;;===
                    MOV DI,BX
                    DEC CX
                    ;;
                    DEC DI
                    JNZ multiplicar_operandos 
                    ;;;
                    INC AUX   
                    DEC CX
                    DEC SI
                    JNS siguiente_multiplicando                      
                    

            ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;       
            
            ;Ajustar acarreos        
        
            MOV SI,017D  
        
            siguiente_acarreo_multiplica:
        
            MOV AL,ENTEROST[SI]
            CMP AL,0AH
            JL sin_acarreo_multiplica 
            AAM
            MOV ENTEROST[SI],AL
            ADD ENTEROST[SI-1H],AH

            sin_acarreo_multiplica:  
            DEC SI
            JNS siguiente_acarreo_multiplica
            RET
        
    MULTIPLICA ENDP
;====================================[USAR_N1]====================================    
    USAR_N1 PROC NEAR 
    MOV SI,018D ;DESPLAZAMIENTO POR EL CUAL SE ALCANZA LA ULTIMA CIFRA CONTANDO A CERO HASTA VARIABLE SIGNO
    ADD SI,DX 
    MOV DI,018D ;POSICION DEL ULTIMO NUMERO DEL DESTINO
    
    cpy_n1:
    MOV AL,[SI] 
    MOV SGN1[DI],AL
    
    DEC SI
    DEC DI
    JNS cpy_n1   
    
    RET    
    USAR_N1 ENDP 
        
;====================================[USAR_N2]====================================
    USAR_N2 PROC NEAR
    MOV SI,018D ;DESPLAZAMIENTO POR EL CUAL SE ALCANZA LA ULTIMA CIFRA CONTANDO A CERO HASTA VARIABLE SIGNO
    ADD SI,DX 
    MOV DI,018D ;POSICION DEL ULTIMO NUMERO DEL DESTINO
    
    cpy_n2:
    MOV AL,[SI] 
    MOV SGN2[DI],AL
    
    DEC SI
    DEC DI
    JNS cpy_n2   
    
    RET       
    USAR_N2 ENDP 
;====================================[SALVAR_TEMPORAL]====================================    
    SALVAR_TEMPORAL PROC NEAR
    MOV SI,018D ;Posicion del ultimo digito decimal
    MOV DI,018D
    ADD DI,DX 
        
    cpy_temp:
    MOV AL,SIGNOT[SI] 
    MOV [DI],AL
    
    DEC DI
    DEC SI
    JNS cpy_temp   
    
    RET       
    SALVAR_TEMPORAL ENDP
    
;====================================[LIMPIAR]====================================    
    LIMPIAR PROC NEAR  
        MOV SI,018D ;Posicion del ultimo digito decimal
        MOV CX,DX
        
        limpiar_siguienteT: 
        MOV DI,SI
        ADD DI,CX 
        MOV AL,0H
        MOV [DI],AL
        ;MOV SIGNOT[SI],0H  
        
        DEC SI
        JNS limpiar_siguienteT 
        RET
    LIMPIAR ENDP 
;====================================[MOSTRAR_RESULTADO]==================================== 
    MOSTRAR_RESULTADO PROC NEAR
        ;REVISAR EL SIGNO
        MOV AL,[DI] ;SIGNOT
        INC DI
        CMP AL,0FFH
        JNE no_imprime_signo
            ;IMPRIME SIGNO 
            MOV DL,'-'
            CALL IMPRIME_CARACTER
            ;
        no_imprime_signo:
        ;IMPRIME ENTEROS
        MOV SI,0FFFFH
            ;MIENTRAS SEA CERO A LA IZQUIERDA EVITAR
            siguiente_numero_entero:
            INC SI 
            ;;;;;;;;;;
            CMP SI,09H ;Ultima localidad del numero entero +1
            JE un_cero_entero
            ;;;;;;;;;; 

            ;;;;;;
            MOV CX,DI
            ADD CX,SI
            MOV BX,SI      
            MOV SI,CX     
            ;;;;;;
            MOV DL,[SI];ENTEROST[SI]  
            MOV SI,BX
            
            
            CMP DL,0H 
            JE siguiente_numero_entero
            ;YA NO ES CERO A LA IZQUIERDA
            CMP SI,09H ;SI SOLO TIENE DECIMALES - Ultima localidad del numero entero +1
            JE un_cero_entero
            ;     
                imprime_enteros:
                
                
                ;;;;;;
                MOV CX,DI
                ADD CX,SI
                MOV BX,SI      
                MOV SI,CX     
                ;;;;;;
                MOV DL,[SI];ENTEROST[SI]  
                MOV SI,BX
                
                ADD DL,30H
                CALL IMPRIME_CARACTER  
                INC SI
                CMP SI,08H ;Ultimo numero entero
                JA imprime_decimales
                JMP imprime_enteros
            ; 
            un_cero_entero: 
            MOV DL,30H
            CALL IMPRIME_CARACTER
            
           imprime_decimales:
           ;REVISAR SI TIENE DECIMALES PARA IMPRIMIR EL PUNTO
            MOV SI,08H ;Ultimo numero entero
            
            siguiente_cero_decimal:
            INC SI ;incremento en 1 para entrar en el campo de los enteros
            CMP SI,017D ;Ultimo numero decimal
            JA sin_decimales 
            
            
                ;;;;;;
                MOV CX,DI
                ADD CX,SI
                MOV BX,SI      
                MOV SI,CX     
                ;;;;;;
                MOV DL,[SI];DECIMALEST[SI]  
                MOV SI,BX
                
                
            CMP DL,0H
            JE siguiente_cero_decimal
            
            ;con decimales
            MOV DL,'.'
            CALL IMPRIME_CARACTER    
            MOV SI,08H ;Ultimo numero entero -1
            
            imprime_decimal_siguiente:
            INC SI


                ;;;;;;
                MOV CX,DI
                ADD CX,SI
                MOV BX,SI      
                MOV SI,CX     
                ;;;;;;
                MOV DL,[SI];DECIMALEST[SI]  
                MOV SI,BX
                

            ADD DL,30H
            CALL IMPRIME_CARACTER
            CMP SI,017D ;Ultimo numero decimal
            JNE imprime_decimal_siguiente
            
        
    sin_decimales:    
    RET 
    MOSTRAR_RESULTADO ENDP  
;====================================[IMPRIME_CARACTER]==================================== 
    IMPRIME_CARACTER PROC NEAR
       MOV AH,02H
       INT 21H          
    RET
    IMPRIME_CARACTER ENDP
;==============>[ALOGAR_CADENA]<==============
        ALOGAR_CADENA PROC NEAR 
    
        ;MOV BLOQUE_ACTUAL,0AH ;INDICA EL BLOQUE EN EL QUE SE ESTA ESCRIBIENDO
    
        ;RESERVAR LA MEOMRIA PARA LEER LA CADENA (MALLOC)
        ;FUNCION AH,48H       
    
        ;PARAMETROS DE ENTRADA:
        ;BX - TAMANIO DE EL BLOQUE (EN PARRAFOS GRUPOS DE 16 BYTES)    
    
        ;PARAMETOS DE SALIDA:  
        ;SI NO HAY ERROR BANDERA DE ACARREO LIMPIA
        ;AX - APUNTA A LA DIRECCION DE INICIO DEL NUEVO BLOQUE RESERVADO
    
        ;SI HAY ERROR ACARREO CON VALOR,
        ;BX - TAMANIO MAXIMO DE MEMORIA DISPONIBLE
        ;AX CODIGO DE ERROR (7 O 8)
              
        MOV AH,48H
        XOR BX,BX
        MOV BL,BLOQUE_ACTUAL ;16*BLOQUE_ACTUAL BYTES DE INICIO
        INT 21H 
        MOV CADENA,AX 
        JC ERROR_ALLOC
        JMP FIN_ALOGAR_CADENA
 
ERROR_ALLOC:     
        MOV CADENA,DS
        XOR AX,AX
        MOV AL,BLOQUE_ACTUAL
        MOV AH,0FH
        MUL AH 
          
        MOV DI,CADENA
        ADD DI,AX
        MOV CX,ULTIMO_SEGMENTO
        ADD DI,CX
        MOV CADENA,DI
    
        ;MOV AH,09H
        ;LEA DX,ERROR_ALLOC_CADENA
        ;INT 21H    
    
        FIN_ALOGAR_CADENA:
        MOV CX,CADENA
        MOV ULTIMO_SEGMENTO,CX
        RET
         
    ALOGAR_CADENA ENDP

        ;==============>[ALOGAR_CADENA]<==============
        LIBERAR_CADENA PROC NEAR ;(FREE)
            ;Funcion AH   49h  
            ;PARAMETROS DE ENTRADA: ES:0-DIRECCION BASE CERO DEL BLOQUE PARA LIBERAR
            ;PARAMETROS DE SALIDA: SI EL ACARREO ESTA ACTIVO AX CONTIENE EL CODIGO DE 
            ;ERROR (7,9)
      
            MOV AH,49H 
            MOV ES,BX
            INT 21H
   
            RET
            LIBERAR_CADENA ENDP 
LEER_CADENA PROC NEAR 
        XOR CX,CX
      
 
     
        MOV CADENA,DS
        XOR AX,AX
        MOV AL,BLOQUE_ACTUAL
        MOV AH,0FH
        MUL AH 
          
        MOV DI,CADENA
        ADD DI,AX
        MOV CX,ULTIMO_SEGMENTO
        ADD DI,CX
        MOV CADENA,DI
        XOR CX,CX 
        ;MOV AH,09H
        ;LEA DX,ERROR_ALLOC_CADENA
        ;INT 21H    
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;LECTURA
AGREGAR_CARACTER:  
    MOV SI,CADENA
    MOV AH,08H
    INT 21H
    
    CMP AL,08H
    JE RETROCEDER
    CMP AL,0DH ;TECLA ENTER
    JE LEER_CADENA_SALIR
    
    MOV DI,SI
    ADD DI,CX 
    MOV [DI],AL 
    INC CX
    
    ;IMPRIMIR EL CARACTER EN PANTALLA
    MOV DL,AL
    MOV AH,02H
    INT 21H
    ;;REVISAR SI YA NOS ACABAMOS EL BLOQUE
    
    MOV dx,0 ; 
    MOV AX,CX ; 
    MOV BX,0FH
    DIV BX ;
    
    CMP AL,BLOQUE_ACTUAL 
    JA REALLOC_CADENA
    JMP AGREGAR_CARACTER
    ;;;;;;;;;;;;;;;;;;;;; 
            REALLOC_CADENA:
        ;MOV AH,09H
        ;LEA DX,REALLOC
        ;INT 21H
        ;;AUMENTAR EL BLOQUE Y RESERVAR UN PARRAFO MAS (REALLOC) 
        ;FUNCION AH,04AH    
        
        ;PARAMETROS ENTRADA
        ;ES - DIRECCION DEL BLOQUE A MODIFICAR
        ;BX NUEVO TAMANIO DE BLOQUE  
        
        ;PARAMETROS DE SALIDA
        ;SI HAY ERROR ACARREO CON VALOR,
        ;BX - TAMANIO MAXIMO DE MEMORIA DISPONIBLE
        ;AX CODIGO DE ERROR (7,8,9)        
            
            INC BLOQUE_ACTUAL
            
            MOV AH,04AH
            MOV ES,DI
            MOV BL,BLOQUE_ACTUAL
            INT 21H
            ;JC ERROR_REALLOC
            JMP AGREGAR_CARACTER
            ERROR_REALLOC:
            LEA DX,ERROR
            MOV AH,09H
            INT 21H 
    ;;;;;;;;;;;;;;;;;;;;;;     
    ;;;;;;;;;;;;;
    RETROCEDER:
    MOV AH,02H
    MOV DL,08H ;backspace 
    INT 21H 
    ;
    MOV AH,02H  
    MOV DL,00H ;caracter nulo para borra el caracter
    INT 21H
    ;
    MOV AH,02H  
    MOV DL,08H;backspace 
    INT 21H
    
    CMP CX,00H
    JE no_decrementar_posicion
    DEC CX                  
    
    no_decrementar_posicion:

    JMP AGREGAR_CARACTER
    ;;;;;;;;;;;;;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;    
        LEER_CADENA_SALIR:
        
        MOV AL,FIN_CADENA
        MOV DI,SI
        ADD DI,CX         
        MOV [DI],AL 
        INC CX
    
        MOV CX,CADENA
        MOV ULTIMO_SEGMENTO,CX
   
   RET      
   LEER_CADENA ENDP
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
        CMP SI,0012 ;Posicion del ultimo digito decimal ~
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
        CMP SI,018D ;Posicion del ultimo digito decimal ~
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

MENOR_QUE PROC NEAR

CALL MAYOR_QUE
MOV AL,R_COMPARADOR
CMP AL,01H
JE A_CERO 
JMP A_UNO

A_CERO:
MOV R_COMPARADOR,0H
JMP SALIR_MENOR_QUE

A_UNO:
MOV R_COMPARADOR,1H

SALIR_MENOR_QUE:
RET   
MENOR_QUE ENDP

MENOR_QUE_IGUAL PROC NEAR

CALL MENOR_QUE  
MOV AL,R_COMPARADOR
CMP AL,01H
JE VERDADERO
CALL IGUAL
MOV AL,R_COMPARADOR
CMP AL,01H
JE VERDADERO
JMP FALSO
 
VERDADERO:
MOV R_COMPARADOR,01H
JMP FIN_COMPARACION
FALSO:
MOV R_COMPARADOR,0H

FIN_COMPARACION: 
RET
MENOR_QUE_IGUAL ENDP

IGUAL_CADENA PROC NEAR
    MOV R_COMPARADOR,1H
    
    MOV CX,0FFFFH
    XOR AX,AX
siguiente_iguales_cadena: 
    CMP AL,FIN_CADENA
    JE fin_iguales_cadena 
       
    INC CX
    
    MOV DI,CAD_TEMP1
    
    MOV SI,CAD_TEMP2
    
    ADD DI,CX
    ADD SI,CX
    
    MOV AL,[DI]
    MOV AH,[SI]
    
    CMP AL,AH
    JE siguiente_iguales_cadena
    JNE diferentes_iguales_cadena
    


    
diferentes_iguales_cadena:
MOV R_COMPARADOR,0H    

fin_iguales_cadena:
RET    
IGUAL_CADENA ENDP
DIFERENTE_CADENA PROC NEAR
CALL IGUAL_CADENA

MOV AL,R_COMPARADOR
CMP AL,01H
JE A_CERO_C 
JMP A_UNO_C

A_CERO_C:
MOV R_COMPARADOR,0H
JMP SALIR_DIFERENTE_C

A_UNO_C:
MOV R_COMPARADOR,1H

SALIR_DIFERENTE_C:
RET   
DIFERENTE_CADENA ENDP
LEER_DECIMAL PROC NEAR 
    XOR CX,CX ;CUENTA EL NUMERO DE ENTEROS
    MOV SI,DI
    MOV AH,01H
    INT 21H
              
    MOV AH,'-'          
    CMP AL,AH    
    JE lectura_entero_negativo
    MOV AH,'+'
    CMP AH,AL
    JE lectura_decimal_positivo
    JMP lectura_decimal_positivo_usgn
    
    lectura_decimal_positivo:
    MOV DL,01H 
    MOV [DI],DL
    JMP lectura_numeros
    
    lectura_decimal_positivo_usgn:
    MOV DL,01H
    MOV [DI],DL
    INC DI  
    INC CX
    JMP validar_numero_entero
lectura_entero_negativo:
    MOV DL,0FFH
    MOV AL,DL 
    MOV [DI],AL   
    
    JMP lectura_numeros
    back_space_lectura_numeros_decimales:    
    DEC CX
    CMP CX,00H
    JE lectura_numeros
    DEC DI
    MOV [DI],00H
    DEC CX
    DEC DI
    
    ;NUMEROS
    lectura_numeros:
    INC CX
    INC DI 
    CMP CX,0BH 
    JAE overflow_numerico 
    
    MOV AH,01H
    INT 21H       
    
    CMP AL,0DH
    JE fin_lectura_entero 
    CMP AL,08H
    JE back_space_lectura_numeros_decimales 
    
    CMP AL,2EH ;'.' 
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
    ADD DI,0AH
    
    lectura_numero_decimal:
    CMP AUXILIAR,09H 
    JA overflow_numerico
    
    MOV AH,01H
    INT 21H
    
    CMP AL,0DH
    JE fin_lectura_entero  
    CMP AL,08H
    JE back_space_decimal_lec
    CMP AL,30H
    JL numero_invalido_enteros
    CMP AL,39H
    JA numero_invalido_enteros   
       
    SUB AL,30H
    MOV [DI],AL
    
    INC AUXILIAR
    INC DI 
    
   JMP lectura_numero_decimal

    back_space_decimal_lec:
    CMP AUXILIAR,00H
    JE lectura_numeros_regresar_enteros
    DEC AUXILIAR
    DEC DI
    MOV [DI],00H ;BORRAR 
    MOV DL,00H
    MOV AH,02H
    INT 21H  
    MOV DL,08H
    MOV AH,02H
    INT 21H
    JMP lectura_numero_decimal
    
    lectura_numeros_regresar_enteros:
    MOV DL,00H
    MOV AH,02H
    INT 21H  
    MOV DL,08H
    MOV AH,02H
    INT 21H
    INT 21H
    MOV DL,00H
    INT 21H
    MOV DL,08H
    INT 21H
    DEC CX
    MOV DI,SI
    ADD DI,CX 
    INC DI
    INC CX
    JMP back_space_lectura_numeros_decimales
      
fin_lectura_entero:
;AJUSTAR LA PARTE DECIMAL
DEC CX
CMP CX,09H;04H
JL  ajuste_entero_necesario
JMP sin_ajuste_entero
ajuste_entero_necesario:  

MOV AL,CL
MOV AH,09H;04H
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
LEER_DECIMAL ENDP

ES_NUMERO PROC NEAR
    
    CMP AL,30H
    JL  numero_invalido_enteros3
    CMP AL,39H
    JA numero_invalido_enteros3 
    
    SUB AL,30H 
    RET
    numero_invalido_enteros3:
    MOV AL,'$'
    
RET    
ENDP
LEER_ENTERO PROC NEAR 

    ;LIMPIAR VARIABLES
    MOV DX,DI
    CALL LIMPIAR

    MOV SI,DI
    XOR CX,CX
    
    MOV DL,01H
    MOV [DI],DL

    MOV AH,01H
    INT 21H
    
    ;REVISAR SIGNO
    MOV AH,'-'
    CMP AH,AL
    JE lectura_entero_negativo2
    MOV AH,'+'
    CMP AH,AL
    JE lectura_entero_positivo2
    JMP validar_numero
    
    lectura_entero_negativo2:
    MOV DL,0FFH
    MOV [DI],DL
    JMP siguiente_entero
    
    lectura_entero_positivo2:
    MOV DL,01H
    MOV [DI],DL
    JMP siguiente_entero
    
    siguiente_entero:
    MOV AH,01H
    INT 21H    
    ;;;;;;;;;
        validar_numero: 
        CMP CX,09H
        JAE overflow_numerico2
            CMP AL,08H
            JE retroceso_entero 
            CMP AL,0DH
            JE fin_ingreso_entero
        CALL ES_NUMERO
        MOV AH,'$'
        CMP AL,AH
        JNE salvar_numero
        JMP numero_invalido_enteros2   
    ;;;;;;;;; 
    salvar_numero:
    INC DI
    INC CX
    MOV [DI],AL
    JMP siguiente_entero
    
    retroceso_entero:
    MOV DL,00H
    MOV AH,02H
    INT 21H
    MOV DL,8H
    INT 21H    
        
    CMP CX,0H
    JE siguiente_entero
    DEC CX
    MOV DL,0H
    MOV [DI],DL
    DEC DI
    JMP siguiente_entero 
    
fin_ingreso_entero:
    ;AJUSTE
    CMP CX,00H
    JE entero_ingreso_vacio
    CMP CX,09H
    JL ajustar_entero
    
    ajustar_entero: 
    MOV AX,SI ;SALVAR DIRECCION
    
    
    MOV DI,SI
    ADD DI,CX ;FUENTE 
    
    ADD SI,09H ;DESTINO
    
    
    ajusta_siguiente_digito_entero:
        
        MOV AL,[DI]
        MOV [SI],AL
        MOV DL,0H
        MOV [DI],DL
        
        DEC SI
        DEC DI   
    
    DEC CX    
    CMP CX,00H
    JE salir_ajuste_entero
    JMP ajusta_siguiente_digito_entero
entero_ingreso_vacio:
salir_ajuste_entero:    
RET       
overflow_numerico2: 
LEA DI,OVERFLOW  
CALL EXCEPCION  
numero_invalido_enteros2:
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
IMPRIME_CADENA PROC NEAR
    
    MOV DI,DX
    
    siguiente_caracter_impresion2:
    MOV DL,[DI]
    CMP DL,015H ;ALIAS IMPRIMIBLE PARA $
    JE imprime_signo_pesos
    CMP DL,'$'
    JE salir_IMPRIME_CADENA
    
    MOV AH,02H
    INT 21H
    
    siguiente_caracter_impresion:
    INC DI
    JMP siguiente_caracter_impresion2
    
    
    imprime_signo_pesos:
    MOV DL,'$'
    MOV AH,02H
    INT 21H
    
    
    JMP siguiente_caracter_impresion
    
salir_IMPRIME_CADENA:
RET    
IMPRIME_CADENA ENDP
begin endp
end begin
