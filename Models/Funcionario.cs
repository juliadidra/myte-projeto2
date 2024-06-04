﻿using System.ComponentModel.DataAnnotations;

namespace myte.Models
{
    public class Funcionario
    {
        //Definir as propriedades
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "O campo Sobrenome é obrigatório.")]
        public string? Sobrenome { get; set; }

        [Required(ErrorMessage = "O campo Data de nascimento é obrigatório.")]
        public DateTime DataDeNascimento { get; set; }

        [Required(ErrorMessage = "O campo e-mail é obrigatório.")]
        [RegularExpression("[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9])+\\.+[a-zA-Z]{2,6}$")]
        public string? Email { get; set; }

        //[Required] a senha precisa ser required, esta comentado so temporariamente
        public string? Password { get; set; }

        [Required(ErrorMessage = "O campo data de contratação é obrigatório.")]
        public DateTime DataDeContratacao { get; set; }

        [Required(ErrorMessage = "O campo genero é obrigatório.")]
        public string? Genero { get; set;}

        [Required(ErrorMessage = "Selecione uma senioridade.")]
        public string? Senioridade { get; set; }

        [Required(ErrorMessage = "Selecione um cargo.")]
        public string? Cargo { get; set; }

        [Required(ErrorMessage = "Selecione um departamento.")]
        public string? Departamento { get; set;}

        [Required(ErrorMessage = "Selecione um tipo de acesso.")]
        public string? Acesso { get; set; }

        /*// Adicione a foto do funcionário
        public byte[]? Foto { get; set; }*/
    }
}
