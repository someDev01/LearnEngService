import { ChevronLeft, MoveLeftIcon } from 'lucide-react';
import styles from '../button_back/button_back.module.css';
import { useNavigate } from 'react-router-dom';

function ButtonBack(){

    const navigation = useNavigate();
    const onBack = () => {navigation('/', {replace: true})}
    
    return(
        <div className={styles.button} onClick={onBack}>
            <ChevronLeft size={24} color='#d2d2d2'/>
            <p>Видео</p>
        </div>
    )
}

export default ButtonBack;