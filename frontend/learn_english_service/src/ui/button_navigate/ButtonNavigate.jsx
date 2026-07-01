import { ChevronRight } from 'lucide-react';
import styles from '../button_navigate/button_navigate.module.css';

function ButtonNavigate({onClick, title, children, type=null, count=null}){
    return(
        <div className={styles.button} onClick={onClick}>
            <div style={{display: 'flex', gap:'10px'}}>
                <div className={styles.svg_block}>
                    {children}
                </div>
                <div className={styles.title}>{title}</div>
                {type === 'dict' && (
                    <div className={styles.count_block}>
                        <p>({count})</p>
                    </div>
                )}
            </div>
            <ChevronRight size={14} color='#c4c4c4'/>
        </div>
    )
}

export default ButtonNavigate;