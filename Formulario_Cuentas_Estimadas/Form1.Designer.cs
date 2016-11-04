namespace Formulario_Cuentas_Estimadas
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.Btn_Prueba = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Btn_Prueba
            // 
            this.Btn_Prueba.Location = new System.Drawing.Point(12, 27);
            this.Btn_Prueba.Name = "Btn_Prueba";
            this.Btn_Prueba.Size = new System.Drawing.Size(173, 42);
            this.Btn_Prueba.TabIndex = 0;
            this.Btn_Prueba.Text = "Prueba";
            this.Btn_Prueba.UseVisualStyleBackColor = true;
            this.Btn_Prueba.Click += new System.EventHandler(this.Btn_Prueba_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 95);
            this.Controls.Add(this.Btn_Prueba);
            this.Name = "Form1";
            this.Text = "Cuentas estimadas";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Btn_Prueba;
    }
}

