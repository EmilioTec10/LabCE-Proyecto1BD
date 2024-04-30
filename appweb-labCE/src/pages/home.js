
import { useNavigate } from 'react-router-dom';

const Home = () => {
  const navigate = useNavigate();

  const handleAdminLogin = () => {
    navigate('/menu_gestion_profesores');
  };

  const handleOperatorLogin = () => {
    navigate('/login_operador');
  };

  const handleProfessorLogin = () => {
    navigate('/login_profesor');
  };

  

  return (
    <div className="mainContainer">
      <div className="titleContainer">
        <div>Bienvenido a LabCE</div>
      </div>
      <div className="buttonContainer">
        
        <input
          className="inputButton"
          type="button"
          onClick={handleOperatorLogin}
          value={'Continuar como operador'}
        />
        <input
          className="inputButton"
          type="button"
          onClick={handleAdminLogin}
          value="Continuar como administrador"
        />
        <input
          className="inputButton"
          type="button"
          onClick={handleProfessorLogin}
          value="Continuar como Profesor"
        />
      </div>
    </div>
  );
};

export default Home;
