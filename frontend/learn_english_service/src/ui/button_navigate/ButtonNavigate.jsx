import { ChevronRight } from 'lucide-react';
import styles from '../button_navigate/button_navigate.module.css';
import SvgBlock from '../svg_block/SvgBlock';

function ButtonNavigate({onClick, title, children, type=null, count=null}){
    return(
        <div className={styles.button} onClick={onClick}>
            <div style={{display: 'flex', gap:'10px'}}>
                <SvgBlock type={type}>
                    {children}
                </SvgBlock>
                <div className={styles.title}>{title}</div>
            </div>
            <div className={styles.count_and_follow}>
                {count > 0 && (
                    <div className={styles.count_block}>
                        <p>{count}</p>
                    </div>
                )}
                {type !== "logout" && <ChevronRight size={12} color='#c4c4c4'/>}
            </div>
        </div>
    )
}

export default ButtonNavigate;