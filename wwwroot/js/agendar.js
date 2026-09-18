window.onload = async () => {
    document.getElementById('selPaciente').innerHTML += '<option value="1">Pedro Henrique Silva</option><option value="2">Lucas Gabriel Silva</option>';
    document.getElementById('selProfissional').innerHTML += '<option value="1">Dr. André Rodrigues (Pediatria)</option><option value="2">Dra. Amanda Cavalcanti (Psicologia)</option>';
    document.getElementById('selSala').innerHTML += '<option value="1">Sala Azul - Integração Sensorial</option><option value="2">Sala Verde - Atendimento Infantil</option>';
};

async function agendar(event) {
    event.preventDefault();

    const dataStr = document.getElementById('dataConsulta').value;
    const horaIniStr = document.getElementById('horaInicio').value;
    const horaFimStr = document.getElementById('horaFim').value;

    const agendamento = {
        PacienteId: parseInt(document.getElementById('selPaciente').value),
        ProfissionalId: parseInt(document.getElementById('selProfissional').value),
        SalaId: parseInt(document.getElementById('selSala').value),
        DataHoraInicio: `${dataStr}T${horaIniStr}:00`,
        DataHoraFim: `${dataStr}T${horaFimStr}:00`,
        Status: 'Confirmado',
        Observacoes: document.getElementById('observacoes').value
    };

    console.log("Payload para API C#:", agendamento);
    alert("Agendamento efetuado com sucesso!");
}