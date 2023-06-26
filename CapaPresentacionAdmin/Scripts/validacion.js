
const nombre = document.getElementById("txtNombre")
const apellido = document.getElementById("txtApellido")
const correo = document.getElementById("txtCorreo")
const usuario = document.getElementById("txtNombUser")
const telefono = document.getElementById("txtTelefono")
const form = document.getElementById("form")
const parrafo = document.getElementById("mensajeError")
let entrar = true

form.addEventListener("submit", e => {
    e.preventDefault()
    let warnings = ""
    entrar = true
    let regexEmail = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,4})+$/
    parrafo.innerHTML = ""
    let trasnf_telf = (parseInt(telefono.value, 10))
    let first_digit = trasnf_telf.toString()
    first_digit = first_digit.charAt(0)

    if (nombre.value.length < 5) {
        warnings += `El nombre no es valido <br>`
        entrar = false
    }
    if (apellido.value.length < 5) {
        warnings += `El apellido no es valido <br>`
        entrar = false
    }
  
    if ((trasnf_telf.toString().length == 8) && first_digit == '2' || first_digit == '7' || first_digit == '8') {
        warnings += ``
        entrar = true
    } else{
        warnings += `El formato del telefono no es valido <br>`
        entrar = false
    }
    if (!regexEmail.test(correo.value)) {
        warnings += `El correo no es valido <br>`
        entrar = false
    }
    if (usuario.value.length < 6) {
        warnings += `El usuario no es valido <br>`
        entrar = false
    }

    if (!entrar) {
        parrafo.innerHTML = warnings
    }
})
