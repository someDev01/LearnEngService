import styles from '../loader_search/loader_search.module.css';

function LoaderSearch(){
    return(
        <div className={styles.wrapper}>
            <div className={styles.loader}></div>
        </div>
    )
}

export default LoaderSearch;