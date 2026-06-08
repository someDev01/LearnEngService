import { Plus } from 'lucide-react';
import styles from '../button_create_note/button_create_note.module.css';

function ButtonCreateNote({onClick}){
    return(
        <div className={styles.button_create} onClick={onClick}>
            <Plus size={18} color='white'/>
            <p>Добавить</p>
        </div>
    )
}

export default ButtonCreateNote;