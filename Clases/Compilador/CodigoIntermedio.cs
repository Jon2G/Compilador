using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ICSharpCode.AvalonEdit.Document;
using My8086.Clases.Fases;
using My8086.Clases.Funciones;

namespace My8086.Clases.Compilador
{
    public class CodigoIntermedio
    {
        internal readonly Programa Programa;
        internal readonly StringBuilder Codigo;
        private Stack<IBloque> BloquesPorCerrar;
        internal CodigoIntermedio(Fases._4._Sintetizador.Sintesis sintetizado)
        {
            this.Codigo = new StringBuilder();
            this.Programa = sintetizado.Programa;
            this.BloquesPorCerrar = new Stack<IBloque>();

        }
        public void Generar()
        {
            this.Codigo.AppendLine(".model small");
            this.Codigo.AppendLine(".stack");
            this.Codigo.AppendLine(".386");
            this.Codigo.AppendLine(".data");
            this.Codigo.AppendLine(";=========================================================");
            this.Codigo.AppendLine(";VARIABLES TEMPORALES PARA LAS OPERACIONES ARITMETICAS");
            this.Codigo.AppendLine(";PRIMER OPERADOR");
            this.Codigo.AppendLine("SGN1 db 0");
            this.Codigo.AppendLine("NUM1 db 0,0,0,0");
            this.Codigo.AppendLine("DEC1 db 0,0,0,0 ");
            this.Codigo.AppendLine(";SEGUNDO OPERADOR");
            this.Codigo.AppendLine("SGN2 db 0");
            this.Codigo.AppendLine("NUM2 db 0,0,0,0");
            this.Codigo.AppendLine("DEC2 db 0,0,0,0");
            this.Codigo.AppendLine(";VARIABLES DE RESULTADOS GLOBAL");
            this.Codigo.AppendLine("SIGNOT     db 0");
            this.Codigo.AppendLine("ENTEROST   db 0,0,0,0");
            this.Codigo.AppendLine("DECIMALEST db 0,0,0,0");
            this.Codigo.AppendLine(";AUXILIARES GLOBALES PARA LAS OPERACIONES");
            this.Codigo.AppendLine("TEMP db 0,0,0,0,0,0,0,0,0");
            this.Codigo.AppendLine("AUX  dw 0");
            this.Codigo.AppendLine("POSICION_DIV dw 0");
            this.Codigo.AppendLine("OVERFLOWDIV dw 0");
            this.Codigo.AppendLine("DIVISON dw 0,0,0");
            this.Codigo.AppendLine(";=========================================================");
            this.Codigo.AppendLine(";VARIABLES PARA LA LEECTURA DE CADENAS");
            this.Codigo.AppendLine("CADENA DW 00");
            this.Codigo.AppendLine("LONGUITUD_CADENA DW 0");
            this.Codigo.AppendLine("BLOQUE_ACTUAL DB 0");
            this.Codigo.AppendLine("ULTIMO_SEGMENTO DW 0");
            this.Codigo.AppendLine("ERROR DB 'ERROR','$'");
            this.Codigo.AppendLine("REALLOC DB 10,13,'REALLOC',10,13,'$'");
            this.Codigo.AppendLine("ERROR_ALLOC_CADENA DB 10,13,'OCURRIO UN ERROR RESERVANDO MEMORIA PARA LA CADENA',10,13,'$'");
            this.Codigo.AppendLine("FIN_CADENA DB '$'");
            this.Codigo.AppendLine(";=========================================================");
            this.Codigo.AppendLine(";VARIABLES DEPENDIENTES DE LA PROGRAMACION");
            this.Codigo.Append(this.Programa.SegmentoDeDatos.Traduccion());
            this.Codigo.AppendLine(".code");
            this.Codigo.AppendLine(";INICIA PROCEDIMIENTO PRINCIPAL");
            this.Codigo.AppendLine("begin proc FAR");
            this.Codigo.AppendLine("MOV AX,@data");
            this.Codigo.AppendLine("MOV DS,AX");
            this.Codigo.AppendLine(";================>[LIMPIAR PANTALLA]<================");
            this.Codigo.AppendLine(@"mov ax,0600h  ;ah 06(es un recorrido), al 00(pantalla completa)");
            this.Codigo.AppendLine(@"mov bh,01Fh    ;fondo blanco(7), sobre azul(1)");
            this.Codigo.AppendLine(@"mov cx,0000h  ;es la esquina superior izquierda reglon: columna");
            this.Codigo.AppendLine(@"mov dx,184Fh ;es la esquina inferior derecha reglon: columna");
            this.Codigo.AppendLine(@"int 10h  ;interrupcion que llama al BIOS");
            this.Codigo.AppendLine(";================>[CODIGO GENERADO POR EL COMPILADOR]<================");
            foreach (Accion accion in this.Programa.Acciones)
            {
                AgregarAccion(accion);
            }
            this.Codigo.AppendLine(";==============>[FIN CODIGO GENERADO POR EL COMPILADOR]<==============");
            this.Codigo.AppendLine("XOR AX,AX");
            this.Codigo.AppendLine("INT 16H");
            this.Codigo.AppendLine(";================>[LIBERAR TODA LA MEMORIA DE CADENAS]<================");
            this.Codigo.Append(this.Programa.SegmentoDeDatos.Free());

            this.Codigo.AppendLine("MOV AH,4CH");
            this.Codigo.AppendLine("INT 21H");
            this.Codigo.AppendLine("RET");
            this.Codigo.AppendLine("");

            #region Procedimientos

            this.Codigo.AppendLine(";==============>[PROCEDIMIENTOS INTERNOS GENERADOS POR EL COMPILADOR]<==============");
            this.Codigo.AppendLine(@";====================================[SUMA]====================================        
    SUMA PROC NEAR
    ;LIMPIAR VARIABLES
    LEA DX,SIGNOT
    CALL LIMPIAR
    ;        
    ;COMPROBAR SI SE DEBERIA USAR LA RESTA EN SU LUGAR (-)(-) 
    MOV AL,SGN1
    CMP AL,SGN2
    JE  signos_iguales_suma
    JMP signos_diferentes_suma 
    
    signos_iguales_suma:
    CMP SGN1,0FFH
    JE sumar_y_negar  
    JNE sumar_con_normalidad
    
    signos_diferentes_suma:
    CMP SGN2,0FFH
    ;si el segundo es negativo pero el primero no,podemos restar tal cual :)
    JNE revisar_signo1_suma
    CALL RESTA
    RET
    revisar_signo1_suma:
    ;si el primero es negativo pero el segundo no,debemos invertir los operandos
    ;y restar normalmente...
        MOV SI,08H

            invierte_siguiente_suma:
            ;
            MOV AL,SGN1[SI]
            MOV AH,SGN2[SI] 
            
            MOV SGN2[SI],AL  
            MOV SGN1[SI],AH
            ;     
            DEC SI
            JNS invierte_siguiente_suma
        
        CALL RESTA
        
    RET
    sumar_y_negar:
    MOV SIGNOT,0FFH
    sumar_con_normalidad:   
            ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
            MOV SI,5H ;NUMERO MAS A LA IZQUIERDA             
        
                siguiente_sumando:
            

                    MOV AL,NUM2[SI] ;OP_1
                    ADD AL,NUM1[SI]
                    AAM
                    ;;===
                        ;

                        ADD AL,ENTEROST[SI]           
                        MOV ENTEROST[SI],AL

                   
                        ADD AH,ENTEROST[SI-1]
                        MOV ENTEROST[SI-1],AH  
                    
                        ;
                    ;;===
                    DEC SI
                    JNS siguiente_sumando                      
                    

            ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;       
            
            ;Ajustar acarreos        
        
            MOV SI,07H  
        
            siguiente_acarreo:
        
            MOV AL,ENTEROST[SI]
            CMP AL,0AH
            JL sin_acarreo 
            AAM
            MOV ENTEROST[SI],AL
            ADD ENTEROST[SI-1H],AH

            sin_acarreo:  
            DEC SI
            JNS siguiente_acarreo
            RET
        
    SUMA ENDP
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
        MOV SI,08H    
        
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
        MOV NUM1[2],1
        MOV NUM1[3],0 
        
        MOV DEC1[0],0
        MOV DEC1[1],0
        MOV DEC1[2],0
        MOV DEC1[3],0
        
        CALL MULTIPLICA
        
        LEA DX,TEMP
        CALL USAR_N2 
        
        LEA DX,SIGNOT
        CALL USAR_N1
        
        ADD POSICION_DIV,2H
        CMP POSICION_DIV,06H
        JAE fin_division
        ;;;;;;;;;;;;;;;;;;REVISAR QUE EL NUM1 NO SEA YA CERO :V
        MOV SI,07H  
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
            MOV ENTEROST[2H],AH
            MOV ENTEROST[3H],AL
            
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
        JA acomodo_terminado  
        INC SI
        CMP SI,08H
        JE acomodo_terminado
        JMP acomodar_siguiente 

    
    invertir_operandos:
    MOV SIGNOT,0FFH
    MOV SI,7H     
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
            MOV SI,5H ;NUMERO MAS A LA IZQUIERDA             
        
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
        
            MOV SI,07H  
        
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
            MOV SI,5H ;NUMERO MAS A LA IZQUIERDA             
        
                siguiente_multiplicando:
                MOV DI,5H ;NUMERO MAS A LA IZQUIERDA
                MOV CX,7H
                SUB CX,AUX
            
                 multiplicar_operandos:
                    MOV AL,NUM2[SI] ;OP_1
                    MUL NUM1[DI]
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
        
            MOV SI,07H  
        
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
    MOV SI,08H
    ADD SI,DX 
    MOV DI,08H
    
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
    MOV SI,08H
    ADD SI,DX 
    MOV DI,08H
    
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
    MOV SI,08H
    MOV DI,08H
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
        MOV SI,08H 
        MOV CX,DX
        
        limpiar_siguienteT: 
        MOV DI,SI
        ADD DI,CX    
        MOV [DI],0H
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
            CMP SI,04H ;SI SOLO TIENE DECIMALES
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
                CMP SI,03H
                JA imprime_decimales
                JMP imprime_enteros
            ; 
            un_cero_entero: 
            MOV DL,30H
            CALL IMPRIME_CARACTER
            
           imprime_decimales:
           ;REVISAR SI TIENE DECIMALES PARA IMPRIMIR EL PUNTO
            MOV SI,03H
            
            siguiente_cero_decimal:
            INC SI
            CMP SI,5H
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
            MOV SI,03H
            
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
            CMP SI,05H
            JNE imprime_decimal_siguiente
            
        
    sin_decimales:    
    RET 
    MOSTRAR_RESULTADO ENDP  
;====================================[IMPRIME_CARACTER]==================================== 
    IMPRIME_CARACTER PROC NEAR
       MOV AH,02H
       INT 21H     
        
    RET
    IMPRIME_CARACTER ENDP");
            this.Codigo.AppendLine(@"
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
         
    ALOGAR_CADENA ENDP");
            this.Codigo.AppendLine(@"
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
            LIBERAR_CADENA ENDP ");

            #endregion
            this.Codigo.AppendLine("begin endp");
            this.Codigo.AppendLine("end begin");
            this.Codigo.AppendLine("");
            this.Codigo.AppendLine("");
            Clipboard.SetText(this.Codigo.ToString());







            //this.Codigo.Append($"namespace {this.Programa.FuncionPrincipal.Titulo} {{"); //namespace
            //this.Codigo.Append($"class {this.Programa.FuncionPrincipal.Titulo} {{"); //clase principal
            //GenerarFuncionPrincipal(this.Programa.FuncionPrincipal);
            //foreach (Funcion Fx in
            //    this.Programa.Funciones.Where(x => x != this.Programa.FuncionPrincipal))
            //{
            //    GenerarFuncion(Fx);
            //}

            //while (this.BloquesPorCerrar.Any())
            //{
            //    this.Codigo.Append(this.BloquesPorCerrar.Pop().CerrarBloque());
            //}
            //this.Codigo.Append("}}"); //namespace,clase principal
            //AgregarLibrerias();
        }
        //private void GenerarFuncionPrincipal(Funcion Fx)
        //{
        //    this.Codigo.AppendLine("static void Main(string[] args){");
        //    this.Codigo.AppendLine("try {");
        //    foreach (Accion accion in Fx.Acciones)
        //    {
        //        AgregarAccion(accion, Fx);
        //    }
        //    this.Codigo.AppendLine("}catch(Exception ex){Console.Clear();Console.WriteLine(\"Excepción no controlada X.x:\"); Console.WriteLine(ex.Message);}");
        //    this.Codigo.AppendLine("Console.ReadKey();}");
        //}
        //private void GenerarFuncion(Funcion Fx)
        //{
        //    this.Codigo.AppendLine($"static void {Fx.Titulo}(){{");
        //    foreach (Accion accion in Fx.Acciones)
        //    {
        //        AgregarAccion(accion, Fx);
        //    }
        //    this.Codigo.AppendLine("}");
        //}
        private void AgregarAccion(Accion accion)
        {
            if (this.BloquesPorCerrar.Any())
            {
                if (accion.LineaDocumento.LineNumber >= this.BloquesPorCerrar.Peek().FinBloque.LineNumber)
                {
                    this.Codigo.Append(this.BloquesPorCerrar.Pop().CerrarBloque());
                }
            }
            this.Codigo.Append(accion.Traduccion());
            if (accion is IBloque bloque)
            {
                BloquesPorCerrar.Push(bloque);
            }
        }
    }
}
