import styles from '../button_load_more/load_more_button.module.css';

function LoadMoreButton({onClick}){
    return(
        <div className={styles.button_more} onClick={onClick}>
            <button>Загрузить еще</button>
        </div>
    )
}

export default LoadMoreButton;