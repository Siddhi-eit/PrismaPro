import React, { useState, useEffect } from 'react';
import { connect } from 'react-redux';
import { useHistory } from 'react-router-dom';
import Grid from 'containers/dispenseHistoryGrid/Grid';
import PageHeader from 'containers/pages/dispenseHistory/PageHeader';
import { saveAs } from 'file-saver';

import {
  getDispenseHistoryList,
  DownloadDispenseExcelFile,
} from 'redux/actions';

const orderOptions = [
  { column: '7', label: 'Tailoring Code' },
  { column: '9', label: 'Amount To Dispense Per Bottle' },
  { column: '1', label: 'Amount To Dispense' },
  { column: '2', label: 'Component Names' },
  { column: '3', label: 'Component Amounts' },
  { column: '4', label: 'Component Unit' },
  { column: '5', label: 'Created Date' },
  { column: '6', label: 'Lot Nr' },
  { column: '8', label: 'Is Dispense' },
];
const orderDirection = [
  { sortOrder: 'asc', label: 'A-Z' },
  { sortOrder: 'desc', label: 'Z-A' },
];

const pageSizes = [10, 20, 50, 100];
const DispenseHistory = ({
  match,
  DispenseListAction,
  allDispenseItems,
  loading,
  machineID,
  DownloadDispenseExcelFileAction,
  DispenseExcelData,
}) => {
  const [displayMode, setDisplayMode] = useState('thumblist');
  const [currentPage, setCurrentPage] = useState(1);
  const [selectedPageSize, setSelectedPageSize] = useState(10);
  const [selectedOrderOption, setSelectedOrderOption] = useState({
    column: '5',
    label: 'Created Date',
  });
  const [totalItemCount, setTotalItemCount] = useState(0);
  const [totalPage, setTotalPage] = useState(1);
  const [search, setSearch] = useState('');
  const [selectedItems, setSelectedItems] = useState([]);
  const [items, setItems] = useState([]);
  const [selectedOrderDirection, setSelectedOrderDirection] = useState({
    sortOrder: 'desc',
    label: 'Z-A',
  });

  useEffect(() => {
    setCurrentPage(1);
  }, [selectedPageSize, selectedOrderOption]);
  useEffect(() => {
    async function fetchData() {
      const gridObject = {
        pageSize: selectedPageSize,
        currentPage,
        orderBy: selectedOrderOption.column,
        search,
        orderDirection: selectedOrderDirection.sortOrder,
        machineID: machineID,
      };
      DispenseListAction(gridObject);
    }
    fetchData();
  }, [
    selectedPageSize,
    currentPage,
    selectedOrderOption,
    selectedOrderDirection,
    search,
    machineID,
  ]);
  useEffect(() => {
    if (allDispenseItems != null) {
      setTotalPage(allDispenseItems.totalPage);
      setItems(
        allDispenseItems.data.map((x) => {
          return { ...x };
        })
      );
      setSelectedItems([]);
      setTotalItemCount(allDispenseItems.totalItem);
    }
  }, [
    allDispenseItems,
    selectedPageSize,
    currentPage,
    selectedOrderOption,
    selectedOrderDirection,
    search,
  ]);

  useEffect(() => {
    if (DispenseExcelData != null) {
      // const blob = new Blob([DispenseExcelData.data], {
      //   type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      // });
      // const blobUrl = URL.createObjectURL(blob);
      // // Example: Triggering a download
      // const downloadLink = document.createElement('a');
      // downloadLink.href = blobUrl;
      // downloadLink.download = 'your_excel_file.xlsx';
      // document.body.appendChild(downloadLink);
      // downloadLink.click();
      // document.body.removeChild(downloadLink);
      // // Remember to revoke the Blob URL when you're done with it
      // URL.revokeObjectURL(blobUrl);
    }
  }, [DispenseExcelData]);

  const startIndex = (currentPage - 1) * selectedPageSize;
  const endIndex = currentPage * selectedPageSize;
  const history = useHistory();
  const navigateToDispnseNowpage = () => {
    history.push('/app/dispense/dispenseManage');
  };

  const ExportDispenseData = () => {
    DownloadDispenseExcelFileAction(machineID);
  };

  useEffect(() => {
    document.getElementById('machineDropdown').style.display = 'block';
  }, []);

  return (
    <>
      {loading && (
        <div className="customOverlay">
          <div className="loading" />
        </div>
      )}
      <div className="disable-text-selection">
        <PageHeader
          heading="Dispense History"
          displayMode={displayMode}
          changeDisplayMode={setDisplayMode}
          changeOrderBy={(column) => {
            setSelectedOrderOption(
              orderOptions.find((x) => x.column === column)
            );
          }}
          changePageSize={setSelectedPageSize}
          selectedPageSize={selectedPageSize}
          totalItemCount={totalItemCount}
          selectedOrderOption={selectedOrderOption}
          match={match}
          startIndex={startIndex}
          endIndex={endIndex}
          selectedItemsLength={selectedItems ? selectedItems.length : 0}
          itemsLength={items ? items.length : 0}
          onSearchKey={(e) => {
            if (e.key === 'Enter') {
              setSearch(e.target.value.toLowerCase());
            }
          }}
          orderOptions={orderOptions}
          pageSizes={pageSizes}
          toggleModal={() => {
            navigateToDispnseNowpage();
          }}
          ExportData={() => {
            ExportDispenseData();
          }}
          orderDirection={orderDirection}
          selectedOrderDirection={selectedOrderDirection}
          changeOrderDirection={(sortOrder) => {
            setSelectedOrderDirection(
              orderDirection.find((x) => x.sortOrder === sortOrder)
            );
          }}
        />
        <Grid
          items={items}
          allDispenseItems={allDispenseItems}
          displayMode={displayMode}
          selectedItems={selectedItems}
          // onCheckItem={onCheckItem}
          currentPage={currentPage}
          totalPage={totalPage}
          // onContextMenuClick={onContextMenuClick}
          //  onContextMenu={onContextMenu}
          onChangePage={setCurrentPage}
        />
      </div>
    </>
  );
};

const mapStateToProps = ({ dispense, user }) => {
  const { allDispenseItems, DispenseExcelData, isAddError, loading } = dispense;
  const { machineID } = user;
  return {
    allDispenseItems,
    isAddError,
    loading,
    machineID,
    DispenseExcelData,
  };
};
export default connect(mapStateToProps, {
  DispenseListAction: getDispenseHistoryList,
  DownloadDispenseExcelFileAction: DownloadDispenseExcelFile,
})(DispenseHistory);
