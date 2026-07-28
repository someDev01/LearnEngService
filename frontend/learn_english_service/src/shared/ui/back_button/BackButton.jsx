import { ChevronLeft } from 'lucide-react';
import styles from '../back_button/back_button.module.css';

function BackButton({onClick}){
    return(
        <div className={styles.back_button} onClick={onClick}>
            <ChevronLeft size={20} color='white'/>
        </div>
    )
}

export default BackButton;