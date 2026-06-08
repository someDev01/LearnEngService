import { ListCheck } from 'lucide-react';
import styles from '../button_training/button_training.module.css';

function ButtonTraining({onClick}){
    return(
        <div className={styles.button} onClick={onClick}>
            <ListCheck size={20} color='#c4c4c4'/>
            <div className={styles.training}>Тренировка</div>
        </div>
    )
}

export default ButtonTraining;