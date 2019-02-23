﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SimulationBateriaTorres
{
	public partial class MainForm : Form
	{
		//Eventhandler	
		EventHandler MyEventHandler;
		
		//Bool que controla la carga de bateria
		bool Conectado = false;
		
		
		//Banderas??? - ControlVars
		bool BateriaAdvertencia = false;
		bool BateriaLlena = false;
		bool BateriaVacia = false;
		
		//Clase contenedora de Datos
		DatosEvento Data = new DatosEvento();
	

		
		public MainForm()
		{
			InitializeComponent();
		}
		
		
		
#region MisFunciones
		
		
		//Esta funcion es la que llama el Evento
		//Contiene el Mensaje a mostrar - Controlado por las condiciones en Timer
		//Contiene condicional que apaga el equipo
		void EventoAviso(object Sender, EventArgs e)
		{
			MessageBox.Show(((DatosEvento)Sender).Mensaje);
			
			if(((DatosEvento)Sender).Apagar)
				Apagar();
		}
		
		
		//Funcion de descarga de bateria
		void Descargar(int i)
		{
			PbBateria.Increment(-(TbVolumen.Value + TbBrillo.Value + i));
		}
				

		//Funcion que Apaga el equipo		
		void Apagar()
		{
			Close();
		}
		
#endregion
		
#region DefaultEvents


		//MainFormLoad - Simplemente carga el evento del aviso de bateria
		void MainFormLoad(object sender, EventArgs e)
		{
			MyEventHandler += EventoAviso;
		}
		
		
		//Timer - Nucleo principal del funcionamiento de la pila
		void TimerTick(object sender, EventArgs e)
		{
			//Muestra la hora en la parte inferior
			LabelHora.Text = DateTime.Now.ToString("T");
			
			
			//Bloque que controla la carga y descarga
			if(Conectado)
			{
				BateriaAdvertencia = false;
				BateriaVacia = false;
				
				PbBateria.Increment(50);
			}
			else
			{
				Descargar(1);
				BateriaLlena = false;
			}
			
			
			
			//El siguiente bloque de condiciones es controlado
			//por las "Banderas":
			//BateriaLlena
			//BateriaAdvertencia
			//BateriaVacia
			
			//Condicion que controla la bateria Llena
			if(PbBateria.Value >= 1000 && !BateriaLlena)
			{
				BateriaLlena = true;
				Data.Mensaje = "La bateria esta completamente cargada";
				
				MyEventHandler(Data, null);
			}			
			//Condicion que controla la advertencia de bateria baja
			else if(PbBateria.Value <= 150 && PbBateria.Value > 0 && !BateriaAdvertencia && !Conectado)
			{
				Data.Mensaje = "Conecte el cargador";
				BateriaAdvertencia = true;
				
				MyEventHandler(Data, null);
			}	
			//Condicion que controla la bateria vacia			
			else if(PbBateria.Value == 0 && !BateriaVacia)
			{
				Data.Mensaje = "El equipo se apagara";
				Data.Apagar = true;
				BateriaVacia = true;
				
				MyEventHandler(Data, null);
			}
		 }		

		
		
		//Controla la carga de la beteria (Boton) para conectar/desconectar cargador
		void CbCargadorCheckedChanged(object sender, EventArgs e)
		{
			Conectado = CbCargador.Checked;			
			CbCargador.Text = (Conectado) ? "Desconectar cargador" : "Conectar cargador";
		}
			
		
		//Boton de apagado (Llama al metodo Apagar)
		void BtnApagarClick(object sender, EventArgs e)
		{
			Apagar();
		}
			

		//Boton debug - Descarar rapidamente		
		void BtnDescargaClick(object sender, EventArgs e)
		{
			if(!Conectado)
				Descargar(50);
		}
		
		
		//ScrollVolumen - Cambia el texto del volumen
		void TbVolumenScroll(object sender, EventArgs e)
		{
			LabelVolumen.Text = TbVolumen.Value.ToString();
		}
		
		//ScrollBrillo - Cambia el texto del brillo
		void TbBrilloScroll(object sender, EventArgs e)
		{
			LabelBrillo.Text = TbBrillo.Value.ToString();
		}	

#endregion
	}
}
