const url = "https://localhost:7081/api/Tarefa";
const tarefaModal = document.getElementById('tarefaModal')
const modalForm = tarefaModal.querySelector('#tarefaForm')

let currentTarefa = null;
let tarefasArray = [];

window.onload = function () { 
  getData();
}

async function getData() {
  try {
    const response = await fetch(url);

    const json = await response.json();

    if (!response.ok) {
      let message = "";
      if (json["errors"]) {
        for (const [key, value] of Object.entries(json["errors"])) {
          message += " " + (`${key}: ${value}`);
        }
      }
      else{
        message += json["detail"];
      }

      throw new Error(message);
    }
      
    tarefasArray = json["data"]
    DisplayToDo(tarefasArray);
  } catch (error) {
    DisplayMessage('alert-danger', error.message);
    console.error(error.message);
  }
}

function DisplayToDo() {

  const pendente = document.getElementById('checkbox-pendente');
  const progresso = document.getElementById('checkbox-progresso');
  const concluida = document.getElementById('checkbox-concluida');
  
  document.querySelector('.tarefasTableBody').innerHTML = "";
  for (let i = 0; i < tarefasArray.length; i++) {    
    if (pendente.checked || progresso.checked || concluida.checked) {
      if ((tarefasArray[i].status === 1 && !pendente.checked)  
        || (tarefasArray[i].status === 2 && !progresso.checked) 
        || (tarefasArray[i].status === 3 && !concluida.checked)) {
        continue;
      }
    }
  
    let statusDescricao = "Pendente";

    if (tarefasArray[i].status === 3) 
      statusDescricao = "<p class='text-success fw-bold'>Concluida</p>";
    else if (tarefasArray[i].status === 2) 
      statusDescricao = "<p class='text-warning fw-bold'>Em Progresso</p>";
    else 
      statusDescricao = "<p class='text-danger fw-bold'>Pendente</p>";

    document.querySelector('.tarefasTableBody').innerHTML += `
      <tr>
          <th scope="row align-middle">${tarefasArray[i].id}</th>
          <td class="col fw-bold align-middle">${tarefasArray[i].titulo}</td>
          <td class="col text-center align-middle">
            ${new Date(tarefasArray[i].dataCriacao).toLocaleString("pt-BR")
            .split(',')[0]}</td>
          <td class="col text-center align-middle">
            ${tarefasArray[i].dataConclusao === null? "" : 
            new Date(tarefasArray[i].dataConclusao).toLocaleString("pt-BR")
            .split(',')[0]}</td>
          <td class="col text-center align-middle">${statusDescricao}</td>
          <td class="col text-center justify-content-between align-middle">
      
            <button type="button" class="btn btn-info view-button btn-sm me-md-5
             align-middle" data-bs-toggle="modal" data-bs-target="#tarefaModal" 
             data-bs-whatever="${tarefasArray[i].id}">
              <img src="./assets/image/eye-fill.svg" alt="Detalhes">
            </button>
            
            <button type="button" class="btn btn-primary btn-sm me-md-5 
            align-middle" data-bs-toggle="modal" data-bs-target="#tarefaModal" 
            data-bs-whatever="${tarefasArray[i].id}">
              <img src="./assets/image/pencil-square.svg" alt="Editar">
            </button>
            
            <button type="button" class="btn btn-danger btn-sm align-middle" 
            onclick="DeleteTarefa(${tarefasArray[i].id})">
              <img src="./assets/image/trash3-fill.svg" alt="Excluir">
            </button>
          </td>
      </tr>`; 
    }
}

async function DeleteTarefa(id){
  try {
    const response = await fetch(`${url}/${id}`, {
      method: "DELETE"
    });

    if (!response.ok) {
      let message = "";
      if (json["errors"]) {
        for (const [key, value] of Object.entries(json["errors"])) {
          message += " " + (`${key}: ${value}`);
        }
      }
      else{
        message += json["detail"];
      }

      throw new Error(message);
    }

    const json = await response.json();
    
    DisplayMessage('alert-success', json["message"]);
    getData();
    DisplayToDo();
    return tarefasArray;

  } catch (error) {
    DisplayMessage('alert-danger', error.message);
    console.error(error.message);
  }
}

async function UpdateTarefa(){

  const _tarefa = {
    id: Number(currentTarefa.id),
    titulo: modalForm.querySelector('#tarefa-name').value,
    descricao: modalForm.querySelector('#descricao-input').value,
    dataConclusao:
     modalForm.querySelector('#data-conclusao-input').value === "" ? null : 
     modalForm.querySelector('#data-conclusao-input').value,
    status: Number(
      modalForm.querySelector('input[name="statusRadioButtons"]:checked').value)
  }

  try {
    const response = await fetch(url, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(_tarefa)
    });

    const json = await response.json();
    console.log(json);
    
    if (!response.ok) {

      let message = "";
      if (json["errors"]) {
        for (const [key, value] of Object.entries(json["errors"])) {
         message += " " + (`${key}: ${value}`);
        }
      }
      else{
        message += json["detail"];
      }  

      throw new Error(`${message}`);
    }
    
    DisplayMessage('alert-success', json["message"]);
    getData();
    DisplayToDo();
  } 
  catch (error) {
    DisplayMessage('alert-danger', error.message);
    console.error(error.message);
  }
  finally {
    closeModal()
  }
}

function DisplayMessage(type, message) {
  const alerta = document.getElementById('alerta');

  alerta.innerHTML = `<div class="alert ${type} alert-dismissible fade show" 
  role="alert">
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert" 
        aria-label="Close"></button>
      </div>`
}

function closeModal() {
  const button = document.getElementById('close-button');
  button.click();
}

async function CreateTarefa(){

  const _tarefa = {
    titulo: modalForm.querySelector('#tarefa-name').value,
    descricao: modalForm.querySelector('#descricao-input').value
  }

  try {
    const response = await fetch(url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(_tarefa)
    });

    const json = await response.json();
    
    if (!response.ok) {
      let message = "";
      if (json["errors"]) {
        for (const [key, value] of Object.entries(json["errors"])) {
          message += " " + (`${key}: ${value}`);
        }
      }
      else{
        message += json["detail"];
      }
      
      throw new Error(`Não foi possivel atualizar a tarefa. Detalhes: ${message}`);
    }

    DisplayMessage('alert-success', json["message"]);
    closeModal();
    getData();
    DisplayToDo();
    
    return tarefasArray;

  } catch (error) {
    DisplayMessage('alert-danger', error.message);
    console.error(error.message);
  }
  finally {
    closeModal()
  }
}

if (tarefaModal) {
  tarefaModal.addEventListener('show.bs.modal', event => {
    
    const button = event.relatedTarget
    const modalTitle = tarefaModal.querySelector('.modal-title')
    const novaTarefa = button.id === "new-tarefa-button"

    TrataVisibilidadeModal(novaTarefa);

    if (!novaTarefa) {
      const tarefaId = button.getAttribute('data-bs-whatever')
      modalTitle.textContent = `Tarefa: ${tarefaId}`

      tarefa = tarefasArray.find(tarefa => tarefa.id == tarefaId);
      currentTarefa = tarefa;
  
      TrataPermissaoEdicao(tarefaModal, button);
  
      PreencheDadosTarefa(tarefa);
    }
    else{
      modalTitle.textContent = "Nova Tarefa";
    }
  })
}

function TrataPermissaoEdicao(tarefaModal, button) {
  tarefaModal.querySelector('#data-criacao-input').setAttribute('disabled', true);

  if (button.classList.contains('view-button')){
      tarefaModal.querySelector('#tarefa-name').setAttribute('disabled', true);
      tarefaModal.querySelector('#data-conclusao-input').setAttribute('disabled', true);
      tarefaModal.querySelector('.form-check-label').setAttribute('disabled', true);
      tarefaModal.querySelector('#descricao-input').setAttribute('disabled', true);    
      modalForm.querySelector('#radio-pendente').setAttribute('disabled', true);
      modalForm.querySelector('#radio-progresso').setAttribute('disabled', true);
      modalForm.querySelector('#radio-concluida').setAttribute('disabled', true);
      tarefaModal.querySelector('#save-button').style.display = 'none';
  }
  else{
      tarefaModal.querySelector('#tarefa-name').removeAttribute('disabled');
      tarefaModal.querySelector('#data-conclusao-input').removeAttribute('disabled');
      tarefaModal.querySelector('.form-check-label').removeAttribute('disabled');
      tarefaModal.querySelector('#descricao-input').removeAttribute('disabled');
      modalForm.querySelector('#radio-pendente').removeAttribute('disabled');
      modalForm.querySelector('#radio-progresso').removeAttribute('disabled');
      modalForm.querySelector('#radio-concluida').removeAttribute('disabled');
      tarefaModal.querySelector('#save-button').style.display = 'block';
  }
}

function TrataVisibilidadeModal(novaTarefa){

  if (novaTarefa) {
    tarefaModal.querySelector('#tarefa-name').style.display = 'block';
    tarefaModal.querySelector('#descricao-input').style.display = 'block';
    tarefaModal.querySelector('#save-button').style.display = 'block';
    tarefaModal.querySelector('#tarefa-datacriacao-div').style.display = 'none';
    tarefaModal.querySelector('#tarefa-dataconclusao-div').style.display = 'none';
    tarefaModal.querySelector('#tarefa-status-div').style.display = 'none';
    tarefaModal.querySelector('#save-button').setAttribute("onclick", "CreateTarefa()");
  }
  else {
    tarefaModal.querySelector('#tarefa-name').style.display = 'block';
    tarefaModal.querySelector('#descricao-input').style.display = 'block';
    tarefaModal.querySelector('#tarefa-datacriacao-div').style.display = 'block';
    tarefaModal.querySelector('#tarefa-dataconclusao-div').style.display = 'block';
    tarefaModal.querySelector('#tarefa-status-div').style.display = 'block';
    tarefaModal.querySelector('#save-button').setAttribute("onclick", "UpdateTarefa()");
  }
}

function PreencheDadosTarefa(tarefa){
  modalForm.querySelector('#tarefa-name').value = tarefa.titulo;
  modalForm.querySelector('#data-criacao-input').value = tarefa.dataCriacao
  .toLocaleString().split('T')[0];
  modalForm.querySelector('#data-conclusao-input').value = 
  tarefa.dataConclusao === null? "" : 
  tarefa.dataConclusao.toLocaleString().split('T')[0];
  modalForm.querySelector('#descricao-input').value = tarefa.descricao;
  
  if (tarefa.status === 1) {
    modalForm.querySelector('#radio-pendente').setAttribute('checked', true);  
  }
  else if (tarefa.status === 2) {
    modalForm.querySelector('#radio-progresso').setAttribute('checked', true);  
  }
  else if (tarefa.status === 3) {
    modalForm.querySelector('#radio-concluida').setAttribute('checked', true);  
  }
}