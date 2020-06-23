

# My8086
Una ligera capa de abstracción para el lengiaje ensamlador del procesador 8086
![Alt text](Images/Captura.PNG?raw=true "Captura")

Pemite escribir codigó de alto nivel que será traducido  a lenguaje ensamblador,compilado con Turbo C y Ejecutado.


### Ejemplo

Diseñar el algoritmo correspondiente a un programa que escribe el porcentaje descontado en una compra, introduciendo por teclado el precio del descuento y el precio pagado.
```
Program Nom.Descuento
	
	Vars
		Decimal Importe;
		Entero DescuentoPCT;	
		Entero Descuento;
	Begin
	Imprime('¿Ingrese el importe de la compra:');
	Lee(Importe);
	Imprime('\nIngrese el descuento en porcentaje:');
	Lee(DescuentoPCT);
	
			DescuentoPCT:=DescuentoPCT/100;
			Descuento:=Importe*DescuentoPCT;
		LimpiarPantalla();
		Imprime('-========================================================-\n');
		Imprime('--> Descuento %');
		Imprime(DescuentoPCT);
		Imprime('\n--> Descuento total $');
		Imprime(Descuento);
		Imprime('\n-========================================================-\n');
		Imprime('-----------> Subtotal $');
		Imprime(Importe);
		Importe:=Importe-Descuento;
		Imprime('\n-----------> Total $');
		
		Si(Importe<=0)
			begin
				Imprime('0.00 (GRATIS)');
			end
		Sino
			begin
				Imprime(Importe);
			end	
		Imprime('\n-========================================================-\n');

End;
```

![Alt text](Images/Descuento.PNG?raw=true "Captura")

## Creditos

* García García Jonathan Eduardo
*Guzmán Olvera Jessica
*Gonzalez Santiesteban Santiago
Proyecto escolar realizado para la ESIME Culhuacán 
*Profesor: Mora Jain Jesus
*Materia: Compiladores
