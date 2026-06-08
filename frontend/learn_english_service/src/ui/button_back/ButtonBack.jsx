import { MoveLeftIcon } from 'lucide-react';
import styles from '../button_back/button_back.module.css';
import { useNavigate } from 'react-router-dom';

function ButtonBack(){

    const navigation = useNavigate();
    const onBack = () => {navigation('/', {replace: true})}
    
    return(
        <div className={styles.button} onClick={onBack}>
            <MoveLeftIcon size={26} color='#ff9655'/>
            <p>Вернуться назад</p>
        </div>
    )
}

export default ButtonBack;