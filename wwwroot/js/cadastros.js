function showTab(id, el) {
    document.querySelectorAll('.form-section').forEach(sec => sec.classList.remove('active'));
    document.querySelectorAll('.tab').forEach(t => t.classList.remove('active'));
    document.getElementById(id).classList.add('active');
    el.classList.add('active');
}

async function salvar(event, entidade) {
    event.preventDefault();
    let payload = {};

    if (entidade === 'responsaveis') {
        payload = { Nome: document.getElementById('respNome').value, Telefone: document.getElementById('respTelefone').value, Email: document.getElementById('respEmail').value };
    } else if (entidade === 'pacientes') {
        payload = { Nome: document.getElementById('pacNome').value, DataNascimento: document.getElementById('pacDataNasc').value, ResponsavelId: parseInt(document.getElementById('pacRespId').value) };
    } else if (entidade === 'profissionais') {
        payload = { Nome: document.getElementById('profNome').value, Especialidade: document.getElementById('profEsp').value, Telefone: document.getElementById('profTelefone').value, Ativo: true };
    } else if (entidade === 'salas') {
        payload = { Nome: document.getElementById('salaNome').value, Capacidade: parseInt(document.getElementById('salaCapacidade').value) };
    }

    console.log(`Enviando POST para /api/${entidade}:`, payload);
    alert(`${entidade.toUpperCase()} cadastrado com sucesso!`);
    event.target.reset();
}