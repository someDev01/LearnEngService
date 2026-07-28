import { ChevronDown } from 'lucide-react';
import styles from '../button_load_more/load_more_button.module.css';

function LoadMoreButton({onClick}){
    return(
        <div className={styles.button_more} onClick={onClick}>
            <button>Загрузить еще <ChevronDown size={18} color='rgb(184, 184, 184)'/></button>
        </div>
    )
}

export default LoadMoreButton;