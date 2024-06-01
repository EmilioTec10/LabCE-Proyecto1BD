
import { useNavigate } from 'react-router-dom';

const Home = () => {
  const navigate = useNavigate();

  const handleAdminLogin = () => {
    navigate('/login_operador');
  };

  const handleOperatorLogin = () => {
    navigate('/registro');
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
          value={'Registro'}
        />
        <input
          className="inputButton"
          type="button"
          onClick={handleAdminLogin}
          value="Log In"
        />
      </div>
    </div>
  );
};

export default Home;
