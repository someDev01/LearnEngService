import styles from '../controls/controls.module.css';

function Controls({onToggleEn,onToggleRu, isShowedEn, isShowedRu}){
    return(
        <div className={styles.controls}>
            <div className={styles.subs_switch}>
                <div className={`${styles.btn_en} ${isShowedEn ? styles.active : ''}`} onClick={onToggleEn}>en</div>
                <div className={`${styles.btn_ru} ${isShowedRu ? styles.active : ''}`} onClick={onToggleRu}>ru</div>
            </div>
        </div>
    )
}

export default Controls;